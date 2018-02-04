import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Web3Service } from '../../services/web3.service';

import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-provider-required',
  templateUrl: './provider-required.component.html',
  styleUrls: ['./provider-required.component.css']
})
export class ProviderRequiredComponent implements OnInit {

  private message: string;
  subscription: Subscription;

  constructor(private web3Service: Web3Service, private router: Router) {
    this.subscription = this.web3Service.web3Change$.subscribe(
      item => { this.checkNetworkStatus() });
  }

  ngOnInit() {
    if (this.web3Service.loaded){
      this.checkNetworkStatus();
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  checkNetworkStatus() {
    if (!this.web3Service.hasWeb3Provider()) {
      this.message = "Please, install metamask";
    }
    else if (!this.web3Service.isRinkeby()) {
      this.message = "Please, switch to rinkeby network";
    }
    else {
      this.router.navigate(['home']);
    }
  }

}
