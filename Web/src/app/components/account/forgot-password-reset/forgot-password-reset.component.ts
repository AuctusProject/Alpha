import { Component, OnInit, Input } from '@angular/core';
import { ForgotPasswordReset } from '../../../model/account/forgotPasswordReset';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from "angular2-notifications";
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-forgot-password-reset',
  templateUrl: './forgot-password-reset.component.html',
  styleUrls: ['./forgot-password-reset.component.css']
})
export class ForgotPasswordResetComponent implements OnInit {

  @Input() forgotPasswordReset: ForgotPasswordReset = new ForgotPasswordReset();

  constructor(private accountService: AccountService, private notificationService: NotificationsService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.forgotPasswordReset.code = this.activatedRoute.snapshot.queryParams['c'];
  }

  onResetClick(): void {
    if (!this.forgotPasswordReset.code) {
      this.notificationService.error("Error", "Invalid reset password request.");
    }
    else if (!this.forgotPasswordReset.newPassword || !this.forgotPasswordReset.confirmedPassword) {
      this.notificationService.error("Error", "Both password fields must be filled.");
    }
    else if (this.forgotPasswordReset.newPassword != this.forgotPasswordReset.confirmedPassword) {
      this.notificationService.error("Error", "Passwords must match.");
    }
    else {
      this.accountService.resetPassword(this.forgotPasswordReset.code, this.forgotPasswordReset.newPassword).subscribe(
        response => {
          if (response) {
            this.notificationService.success("Sucess", "Password was reset.");
            setTimeout(() => this.router.navigateByUrl('login'), 3500);
          }
        }
      );
    }
  }
}
