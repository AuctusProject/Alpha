import { Component, OnInit, Input } from '@angular/core';
import { ForgotPasswordReset } from '../../../model/account/forgotPasswordReset';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-forgot-password-reset',
  templateUrl: './forgot-password-reset.component.html',
  styleUrls: ['./forgot-password-reset.component.css']
})
export class ForgotPasswordResetComponent implements OnInit {

  @Input() forgotPassword: ForgotPasswordReset = new ForgotPasswordReset();

  constructor(private accountService: AccountService) { }

  ngOnInit() {
  }
  onResetClick(): void {
      console.log(this.forgotPassword);
      /*this.accountService.login(this.login)
        .subscribe(response => {
          console.log(response);
        });*/
    }
}
