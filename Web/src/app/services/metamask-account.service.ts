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
  private aucBalance: number;

  constructor(private router: Router, private eventsService: EventsService, private web3Service: Web3Service) {
    this.runChecks();
    this.monitoreAccount();
  }

  monitoreAccount() {
    let self = this;
    var accountInterval = setInterval(function () {
      self.web3Service.getAccount().subscribe(
        account => {
          if (self.account != account) {
            self.broadcastAccountChanged(account);
          }
          if (account) {
            self.checkAUCBalance(account);
          }
          self.account = account;
        })
    }, 100);
  }

  runChecks() {
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

  checkLoginConditions() {
    let self = this;
    this.checkAccountAndNetwork().subscribe(
      success => {
        if (success) {
          this.checkAUCBalance(self.account);
        }
      });
  }

  checkAccountAndNetwork(): Observable<boolean> {
    let self = this;
    return new Observable(
      observer => {
        Observable.combineLatest(this.web3Service.getNetwork(), this.web3Service.getAccount())
          .subscribe(function handleValues(values) {
            self.network = values[0];
            self.account = values[1];
            if (!self.network || !self.account) {
              observer.next(false);
              self.broadcastLoginConditionsFail();
            }
            else {
              observer.next(true);
            }
          });
      });
  }

  checkAUCBalance(account) {
    let self = this;
    this.web3Service.getTokenBalance(environment.tokenAddress, account).subscribe(
      balance => {
        self.broadcastLoginConditionsSuccess();
        /*
        if (self.aucBalance != balance) {
          self.broadcastBalanceChanged(balance);
        }
        if (balance < constants.minimumAUCNecessary){
          self.broadcastLoginConditionsFail();
        }
        else {
          self.broadcastLoginConditionsSuccess();
        }*/
        self.aucBalance = balance;
      });
  }

  private broadcastBalanceChanged(balance) {
    this.eventsService.broadcast("balanceChanged", balance);
  }

  private broadcastAccountChanged(account) {
    this.eventsService.broadcast("accountChanged", account);
  }

  private broadcastLoginConditionsFail() {
    this.eventsService.broadcast("loginConditionsFail");
  }

  private broadcastLoginConditionsSuccess() {
    this.eventsService.broadcast("loginConditionsSuccess");
  }

  public getAUCBalance(): number {
    return this.aucBalance;
  }

  public getAccount(): string {
    return this.account;
  }

  public isRinkeby(): boolean {
    return this.network == 4;
  }

  public getNetwork(): number {
    return this.network;
  }

}
