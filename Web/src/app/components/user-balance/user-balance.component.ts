import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { AccountService } from '../../services/account.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { EventsService } from 'angular-event-service';

@Component({
  selector: 'user-balance',
  templateUrl: './user-balance.component.html',
  styleUrls: ['./user-balance.component.css']
})
export class UserBalanceComponent implements OnInit {

  netValue: number;
  availableToInvest: number;

  constructor(private loginService: LoginService, private accountService: AccountService,
    private changeDetector: ChangeDetectorRef, private localStorageService: LocalStorageService,
    private eventsService: EventsService) {

    this.eventsService.on("purchaseSuccessful", this.onPurchaseSuccessful);
  }

  ngOnInit() {
    var loggedIn = this.loginService.isLoggedIn();
    if (loggedIn) {
      this.updateBalance();
    }
  }

  ngOnDestroy() {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.eventsService.destroyListener("purchaseSuccessful", this.onPurchaseSuccessful);
  }

  private updateBalance() {
    this.accountService.getUserBalance().subscribe(balance => {
      if (balance) {
        this.netValue = balance.totalAmount;
        this.availableToInvest = balance.availableAmount;
        this.changeDetector.detectChanges();
      }
    })
  }

  private onPurchaseSuccessful: Function = (payload: any) => {
    this.updateBalance();
  }
}
