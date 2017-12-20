import { Component, OnInit, Input } from '@angular/core';
import { ForgotPasswordEmail } from '../../../model/account/forgotPasswordEmail';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-forgot-password-email',
  templateUrl: './forgot-password-email.component.html',
  styleUrls: ['./forgot-password-email.component.css']
})
export class ForgotPasswordEmailComponent implements OnInit {

  @Input() forgotPassword: ForgotPasswordEmail = new ForgotPasswordEmail();

  constructor(private accountService: AccountService) { }

  ngOnInit() {
  }

   onSendEmailClick(): void {
      console.log(this.forgotPassword);
      /*this.accountService.login(this.login)
        .subscribe(response => {
          console.log(response);
        });*/
    }
}
