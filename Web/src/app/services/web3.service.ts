import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/share';
import {Observer} from 'rxjs/Observer';

declare let window: any;

@Injectable()
export class Web3Service {

  public loaded: boolean; 
  private web3: any;
  private account: string;
  private network: string;

  web3Change$: Observable<any>;
  public _observer: Observer<any>;

  constructor(private router: Router) {
    this.initProvider();
  }

  initProvider() {
    let web3Service = this;
    this.web3Change$ = new Observable(observer =>
      web3Service._observer = observer).share();

    window.addEventListener('load', function () {
      web3Service.loaded = true;
      // Checking if Web3 has been injected by the browser (Mist/MetaMask)
      if (typeof window.web3 !== 'undefined') {
        // Use Mist/MetaMask's provider
        web3Service.web3 = new Web3(window.web3.currentProvider);
        web3Service.checkNetwork();
        web3Service.checkAccount();
      }
      else {
        web3Service.redirect();
      }
    })
  }

  checkAccount() {
    var account = this.web3.eth.accounts[0];
    this.account = account;
    var accountInterval = setInterval(function () {
      if (this.web3.eth.accounts[0] !== account) {
        this.account = this.web3.eth.accounts[0];
      }
    }, 100);
  }

  redirect() {
    this.router.navigate(['required'])
  }

  checkNetwork() {
    let web3Service = this;
    this.web3.version.getNetwork((err, netId) => {
      web3Service.network = netId;
      web3Service._observer.next(null);
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

}
