import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import { EventsService } from 'angular-event-service';

declare let window: any;

@Injectable()
export class Web3Service {

  public loaded: boolean;
  private web3: any;
  private account: string;
  private network: string;

  constructor(private router: Router, private eventsService: EventsService) {
    this.initProvider();
  }

  initProvider() {
    let web3Service = this;

    window.addEventListener('load', function () {
      web3Service.loaded = true;
      if (typeof window.web3 !== 'undefined') {
        web3Service.web3 = new Web3(window.web3.currentProvider);
        web3Service.checkNetwork();
        web3Service.checkAccount();
      }
      else {
        web3Service.eventsService.broadcast('networkCheck', -1);
        web3Service.eventsService.broadcast('accountChange');
        web3Service.redirect();
      }
    })
  }

  checkAccount() {
    let web3Service = this;
    this.account = this.web3.eth.accounts[0];
    this.eventsService.broadcast('accountChange', this.account);
    var accountInterval = setInterval(function () {
      this.web3.eth.getAccounts(function (err, accounts) {
        if (accounts.length > 0) {
          web3Service.account = accounts[0];
          web3Service.eventsService.broadcast('accountChange', accounts[0]);
        }
        else {
          web3Service.redirect();
        }
      });
    }, 100);
  }

  redirect() {
    if (this.router.url != "/required") {
      this.router.navigate(['required'])
    }
  }

  checkNetwork() {
    let web3Service = this;
    this.web3.version.getNetwork((err, netId) => {
      web3Service.network = netId;
      this.eventsService.broadcast('networkCheck', netId);
      switch (netId) {
        case "1":
          web3Service.redirect();
          break
        case "2":
          web3Service.redirect();
          break
        case "3":
          web3Service.redirect();
          break
        case "4":
          console.log('This is the Rinkeby test network.')
          break
        case "42":
          web3Service.redirect();
          break
        default:
          web3Service.redirect();
      }
    })
  }

  public hasWeb3Provider() {
    return this.web3 != null;
  }

  public isRinkeby() {
    return this.network == "4";
  }

  public getAccount() {
    return this.account;
  }

}
