import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { MetamaskService } from '../../services/metamask.service';

import { EventsService } from 'angular-event-service';

export class MetamaskCondition {
  private message: string;
  public status: boolean;

  constructor(message: string, status: boolean) {
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
    new MetamaskCondition("Get 5 AUC tokens", true)
  ];

  constructor(
    private metamaskService: MetamaskService,
    private router: Router,
    private chRef: ChangeDetectorRef,
    private eventsService: EventsService) {
    this.eventsService.on('networkCheck', this.callbackNetworkCheck);
    this.eventsService.on('accountChange', this.callbackAccountChange);
    this.eventsService.on('AUCBalance', this.callbackAUCBalance);
  }

  ngOnInit() {
    if (this.metamaskService.loaded) {
      this.callbackNetworkCheck();
      this.callbackAccountChange();
      this.callbackAUCBalance();
    }
  }

  ngOnDestroy() {
    this.eventsService.destroyListener('networkCheck', this.callbackNetworkCheck);
    this.eventsService.destroyListener('accountChange', this.callbackAccountChange);
    this.eventsService.destroyListener('AUCBalance', this.callbackAUCBalance);

  }

  private callbackNetworkCheck: Function = (payload: any) => {
    this.conditions[0].status = this.metamaskService.hasMetamask;
    this.conditions[1].status = this.metamaskService.isRinkeby();

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private callbackAUCBalance() {

  }

  private callbackAccountChange: Function = (payload: any) => {
    this.conditions[2].status = this.metamaskService.getAccount() != null;

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private satisfyAllConditions() {
    return this.metamaskService.hasMetamask && this.metamaskService.isRinkeby() && this.metamaskService.getAccount() != null;
  }
}
