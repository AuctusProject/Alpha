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

  runChecks() {
    let metamaskAccountService = this;
    this.web3Service.getWeb3().subscribe(
      web3 => {
        metamaskAccountService.hasMetamask = web3 != null;
        if (!web3) {
          metamaskAccountService.broadcastLoginConditionsFail();
        }
        else {
          metamaskAccountService.checkLoginConditions();
        }
      })
  }

  monitoreAccount() {
    let metamaskAccountService = this;
    var accountInterval = setInterval(function () {
      metamaskAccountService.web3Service.getAccount().subscribe(
        account => {
          if (metamaskAccountService.account != account){
            metamaskAccountService.broadcastAccountChanged(account);
          }
          metamaskAccountService.account = account;
        })
    }, 100);
  }

  checkAccountAndNetwork(): Observable<boolean> {
    let metamaskAccountService = this;
    return new Observable(
      observer => {
        Observable.combineLatest(this.web3Service.getNetwork(), this.web3Service.getAccount())
          .subscribe(function handleValues(values) {
            metamaskAccountService.network = values[0];
            metamaskAccountService.account = values[1];
            if (!metamaskAccountService.network || !metamaskAccountService.account) {
              observer.next(false);
              metamaskAccountService.broadcastLoginConditionsFail();
            }
            else {
              observer.next(true);
            }
          });
      });
  }

  checkAUCBalance() {
    let metamaskAccountService = this;
    this.web3Service.getTokenBalance(environment.tokenAddress, metamaskAccountService.account).subscribe(
      balance => {
        metamaskAccountService.aucBalance = balance;
        if (balance < constants.minimumAUCNecessary) {
          metamaskAccountService.broadcastLoginConditionsFail();
        }
      });
  }

  checkLoginConditions() {
    this.checkAccountAndNetwork().subscribe(
      success => {
        if (success) {
          this.checkAUCBalance();
        }
      });
  }

  private broadcastAccountChanged(account){
    this.eventsService.broadcast("accountChanged", account);
  }

  private broadcastLoginConditionsFail() {
    this.eventsService.broadcast("loginConditionsFail");
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

  //   this.web3Service.getWeb3().subscribe(
  //     () => {
  //       this.checkNetwork();
  //     })
  // }

  // checkNetwork() {
  //   this.web3Service.getNetwork().subscribe(
  //     netId => {

  //     });
  // }

}
