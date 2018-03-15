import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { AccountService } from '../../services/account.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { EventsService } from 'angular-event-service';
import { MetamaskAccountService } from '../../services/metamask-account.service';

@Component({
  selector: 'user-balance',
  templateUrl: './user-balance.component.html',
  styleUrls: ['./user-balance.component.css']
})
export class UserBalanceComponent implements OnInit {

  availableAUC: number = 0;
  netValue: number;
  availableToInvest: number;

  constructor(private loginService: LoginService, private accountService: AccountService,
    private changeDetector: ChangeDetectorRef, private localStorageService: LocalStorageService,
    private eventsService: EventsService, private metamaskAccountService : MetamaskAccountService) {

    this.eventsService.on("purchaseSuccessful", this.onPurchaseSuccessful);
    this.eventsService.on("balanceChanged", this.onBalanceChanged);
  }

  ngOnInit() {
    var loggedIn = this.loginService.isLoggedIn();
    if (loggedIn) {
      var loginData = this.loginService.getLoginData();
      this.netValue = loginData.netValue;
      this.availableToInvest = loginData.availableToInvest;
      this.updateBalance();
    }
  }

  ngOnDestroy() {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.eventsService.destroyListener("purchaseSuccessful", this.onPurchaseSuccessful);
    this.eventsService.destroyListener("balanceChanged", this.onBalanceChanged);
  }

  private updateBalance() {
    this.accountService.getUserBalance().subscribe(balance => {
      if (balance) {
        this.netValue = balance.totalAmount;
        this.availableToInvest = balance.availableAmount;
        var loginData = this.loginService.getLoginData();
        loginData.netValue = this.netValue;
        loginData.availableToInvest = this.availableToInvest;
        this.loginService.setLoginData(loginData);
        this.changeDetector.detectChanges();
      }
    })

    this.availableAUC = this.metamaskAccountService.getAUCBalance();
  }

  private onBalanceChanged: Function = (payload: any) => {
    this.availableAUC = this.metamaskAccountService.getAUCBalance();
  }
  
  private onPurchaseSuccessful: Function = (payload: any) => {
    this.updateBalance();
  }
}
