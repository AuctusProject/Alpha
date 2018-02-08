import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MetamaskAccountService } from '../../services/metamask-account.service';

import { environment } from '../../../environments/environment';
import { constants } from '../../util/contants';
import { EventsService } from "angular-event-service";
import { ChangeDetectorRef } from '@angular/core';

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
    new MetamaskCondition("Minimum "+constants.minimumAUCNecessary+" AUC necessary", true)
  ];

  constructor(
    private router: Router, private metamaskAccount: MetamaskAccountService,
    private eventsService: EventsService, private changeDetector: ChangeDetectorRef) {
    this.eventsService.on("loginConditionsFail", this.onLoginConditionsFail);
    this.eventsService.on("accountChanged", this.onAccountChanged);
  }

  ngOnInit() {
    this.checkNetwork();
    this.checkAccount();
    this.checkAUCBalance();
  }

  ngOnDestroy() {

  }

  private onAccountChanged: Function = (account: any) => {
    this.conditions[2].status = account != null;
    this.changeDetector.detectChanges();

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  }

  private onLoginConditionsFail: Function = (payload: any) => {
    this.checkNetwork();
    this.checkAccount();
    this.checkAUCBalance();
  }

  private checkAUCBalance: Function = (payload: any) => {
    this.conditions[3].status = this.metamaskAccount.getAUCBalance() > constants.minimumAUCNecessary;
    this.changeDetector.detectChanges();

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private checkNetwork: Function = (payload: any) => {
    this.conditions[0].status = this.metamaskAccount.hasMetamask;
    this.conditions[1].status = this.metamaskAccount.isRinkeby();
    this.changeDetector.detectChanges();

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private checkAccount: Function = (payload: any) => {
    this.conditions[2].status = this.metamaskAccount.getAccount() != null;
    this.changeDetector.detectChanges();

    if (this.satisfyAllConditions()) {
      this.router.navigate(['home']);
    }
  };

  private satisfyAllConditions(): boolean {
    return this.metamaskAccount.hasMetamask &&
      this.metamaskAccount.isRinkeby() &&
      this.metamaskAccount.getAccount() != null &&
      this.metamaskAccount.getAUCBalance() > constants.minimumAUCNecessary;
  }
}
