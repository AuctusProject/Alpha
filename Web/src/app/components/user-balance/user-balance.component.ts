import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'user-balance',
  templateUrl: './user-balance.component.html',
  styleUrls: ['./user-balance.component.css']
})
export class UserBalanceComponent implements OnInit {

  investedAmount: number;
  availableToInvest: number;

  constructor(private loginService: LoginService, private accountService: AccountService, private changeDetector : ChangeDetectorRef) { }

  ngOnInit() {
    var loggedIn = this.loginService.isLoggedIn();
    if (loggedIn) {
      this.accountService.getUserBalance().subscribe(balance => {
        if (balance) {
          this.investedAmount = balance.investedAmount;
          this.availableToInvest = balance.availableAmount;
          this.changeDetector.detectChanges();
        }
      })
    }
  }
}
