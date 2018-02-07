import { Injectable, OnDestroy } from '@angular/core';
import { Web3Service } from "./web3.service";
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { EventsService } from 'angular-event-service';

@Injectable()
export class MetamaskService {

  public loaded: boolean;
  public hasMetamask: boolean = false;

  constructor(
    private web3: Web3Service,
    private eventsService: EventsService,
    private router: Router) {
    this.eventsService.on('web3Loaded', this.callbackWeb3Loaded);
    this.eventsService.on('networkCheck', this.callbackNetworkCheck);
    this.eventsService.on('accountChange', this.callbackAccountChange);
    // this.eventsService.on('AUCBalance', this.callbackAUCBalance);
  }

  ngOnDestroy() {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.eventsService.destroyListener('web3Loaded', this.callbackWeb3Loaded);
    this.eventsService.destroyListener('networkCheck', this.callbackNetworkCheck);
    this.eventsService.destroyListener('accountChange', this.callbackAccountChange);
  }

  private callbackNetworkCheck: Function = (payload: string) => {
    switch (payload) {
      case "1":
        this.redirect();
        break
      case "2":
        this.redirect();
        break
      case "3":
        this.redirect();
        break
      case "4":
        console.log('This is the Rinkeby test network.')
        break
      case "42":
        this.redirect();
        break
      default:
        this.redirect();
    }
  }

  private callbackAccountChange: Function = (payload: any) => {
    if (!payload){
      this.redirect();
    }
  };

  private callbackWeb3Loaded: Function = (payload: any) => {
    this.loaded = true;
    if (payload) {
      this.hasMetamask = true;
      this.web3.checkNetwork();
      this.web3.checkAccount();
      // this.web3.checkAUCBalance();
    }
    else {
      this.eventsService.broadcast('networkCheck', -1);
      this.eventsService.broadcast('accountChange');
      this.redirect();
    }
  };

  redirect() {
    if (this.router.url != "/required") {
      this.router.navigate(['required'])
    }
  }

  public getAUCBalance(): number {
    this.web3.getTokenBalance(environment.tokenAddress);
    var a = 2;
    return a;
  }

  public isRinkeby() {
    return this.web3.network == "4";
  }

  public getAccount() {
    return this.web3.account;
  }
}
