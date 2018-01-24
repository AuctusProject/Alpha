import { Component, OnInit, Input } from '@angular/core';
import { ForgotPasswordEmail } from '../../../model/account/forgotPasswordEmail';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from "angular2-notifications";

@Component({
  selector: 'app-forgot-password-email',
  templateUrl: './forgot-password-email.component.html',
  styleUrls: ['./forgot-password-email.component.css']
})
export class ForgotPasswordEmailComponent implements OnInit {

  @Input() forgotPassword: ForgotPasswordEmail = new ForgotPasswordEmail();

  constructor(private accountService: AccountService, private notificationService: NotificationsService) { }

  ngOnInit() {
    this.forgotPassword.emailSent = false;
  }

  onSendEmailClick(): void {
    if (!this.forgotPassword.email) {
      this.notificationService.error("Error", "Email must be filled.");
    }
    else {
      this.accountService.recoverPassword(this.forgotPassword.email).subscribe(response => {
        if (response) {
          this.forgotPassword.emailSent = true;
          this.notificationService.info("Verify you mail box", "Follow email instructions.");
        }
      });
    }
  }
}
