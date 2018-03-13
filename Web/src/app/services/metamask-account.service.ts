import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import { EventsService } from 'angular-event-service';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { constants } from '../util/contants';
import { Web3Service } from "./web3.service";

declare let window: any;

@Injectable()
export class MetamaskAccountService {

  public hasMetamask: boolean;
  private account: string;
  private network: number;
  private aucBalance: number = 0;
  private loggedSuccessfully: boolean;

  constructor(private router: Router, private eventsService: EventsService, private web3Service: Web3Service) {
    this.runChecks();
    this.monitoreAccount();
  }

  private monitoreAccount() {
    let self = this;
    var accountInterval = setInterval(function () {
      if (!self.getNetwork()){
        return;
      }
      self.web3Service.getAccount().subscribe(
        account => {
          if (!self.isRinkeby()){
            self.broadcastLoginConditionsFail();
            return;
          }

          if (self.account && self.account != account) {
            self.broadcastAccountChanged(account);
          }
          if (account) {
            self.checkAUCBalance(account);
          }
          self.account = account;
        })
    }, 100);
  }

  private runChecks() {
    let self = this;
    this.web3Service.getWeb3().subscribe(
      web3 => {
        self.hasMetamask = web3 != null;
        if (!web3) {
          self.broadcastLoginConditionsFail();
        }
        else {
          self.checkLoginConditions();
        }
      })
  }

  private checkLoginConditions() {
    let self = this;
    this.checkAccountAndNetwork().subscribe(
      success => {
        if (success) {
          this.checkAUCBalance(self.account);
        }
      });
  }

  private checkAccountAndNetwork(): Observable<boolean> {
    let self = this;
    return new Observable(
      observer => {
        Observable.concat(this.web3Service.getNetwork(), this.web3Service.getAccount())
          .subscribe(function handleValues(values) {
            self.network = values[0];
            self.account = values[1];
            if (!self.network || !self.account || !self.isRinkeby()) {
              observer.next(false);
              self.broadcastLoginConditionsFail();
            }
            else {
              observer.next(true);
            }
          });
      });
  }

  private checkAUCBalance(account) {
    let self = this;
    this.web3Service.getTokenBalance(environment.aucTokenContract.address, account).subscribe(
      balance => {
        if (self.aucBalance != balance) {
          self.broadcastBalanceChanged(balance);
        }
        if (balance < constants.minimumAUCNecessary) {
          self.broadcastLoginConditionsFail();
        }
        else {
          self.broadcastLoginConditionsSuccess();
        }
        self.aucBalance = balance;
      });
  }

  public sendAUC(value: number) : Observable<string> {
    var aucToSendHex = this.web3Service.toHex(this.web3Service.toWei(value.toString()));
    var data = this.web3Service.getContractMethodData(environment.aucTokenContract.abi,
      environment.aucTokenContract.address, "transfer", environment.escrowContract.address, aucToSendHex);
    return this.web3Service.sendTransaction(1, 80000, this.getAccount(), environment.aucTokenContract.address, 0, data, environment.chainId);
  }

  private broadcastBalanceChanged(balance) {
    this.eventsService.broadcast("balanceChanged", balance);
  }

  private broadcastAccountChanged(account) {
    this.eventsService.broadcast("accountChanged", account);
  }

  public broadcastLoginConditionsFail() {
    this.loggedSuccessfully = false;
    this.eventsService.broadcast("loginConditionsFail");
  }

  private broadcastLoginConditionsSuccess() {
    this.loggedSuccessfully = true;
    this.eventsService.broadcast("loginConditionsSuccess");
  }

  public getAUCBalance(): number {
    return this.aucBalance;
  }

  public getAccount(): string {
    return this.account;
  }

  public isRinkeby(): boolean {
    return this.network && this.network == 4;
  }

  public getNetwork(): number {
    return this.network;
  }

  public isLoggedSuccessfully(){
    return this.loggedSuccessfully;
  }

}
