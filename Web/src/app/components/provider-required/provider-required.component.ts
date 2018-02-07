import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { Web3Service } from '../../services/web3.service';

import { EventsService } from 'angular-event-service';

export class MetamaskCondition {
  private message: string;
  public status: boolean;

  constructor(message : string, status: boolean){
    this.message = message;
    this.status = status;
  }
}

@Component({
  selector: 'app-provider-required',
  templateUrl: './provider-required.component.html',
  styleUrls: ['./provider-required.component.css']
})
export class ProviderRequiredComponent implements OnInit {


  private conditions: Array<MetamaskCondition> = [
    new MetamaskCondition("Install metamask", true),
    new MetamaskCondition("Select rinkeby network", true),
    new MetamaskCondition("Unlock account", true),
  ];

  constructor(private web3Service: Web3Service,
    private router: Router,
    private chRef: ChangeDetectorRef,
    private eventsService: EventsService) {
    this.eventsService.on('networkCheck', this.callbackNetworkCheck);
    this.eventsService.on('accountChange', this.callbackAccountChange);
  }

  ngOnInit() {
    if (this.web3Service.loaded) {
      this.callbackNetworkCheck();
      this.callbackAccountChange();
    }
  }

  ngOnDestroy() {
    this.eventsService.destroyListener('networkCheck', this.callbackNetworkCheck);
    this.eventsService.destroyListener('accountChange', this.callbackAccountChange);

  }

  private callbackNetworkCheck: Function = (payload: any) => {
    this.conditions[0].status = this.web3Service.hasWeb3Provider();
    this.conditions[1].status = this.web3Service.isRinkeby();
    
    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private callbackAccountChange: Function = (payload: any) => {
    this.conditions[2].status = this.web3Service.getAccount() != null;
    
    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private satisfyAllConditions() {
    return this.web3Service.hasWeb3Provider() && this.web3Service.isRinkeby() && this.web3Service.getAccount() != null;
  }
}
