import { Component, OnInit, Input } from '@angular/core';
import { ChangePassword } from '../../../model/account/changePassword';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from 'angular2-notifications';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  @Input() changePassword: ChangePassword = new ChangePassword();
  promise: Subscription;

  constructor(private accountService: AccountService, private notificationService: NotificationsService, private router: Router) { }

  ngOnInit() {
  }

  onChangePasswordClick(): void {
    if (!this.changePassword.currentPassword || !this.changePassword.newPassword || !this.changePassword.confirmedPassword) {
      this.notificationService.error("Error", "All fields must be filled.");
    }
    else if (this.changePassword.newPassword != this.changePassword.confirmedPassword) {
      this.notificationService.error("Error", "Passwords must match.");
    }
    else {
      this.promise = this.accountService.changePassword(this.changePassword.currentPassword, this.changePassword.newPassword).subscribe(
        response => {
          if (response) {
            this.notificationService.success("Sucess", "Password was changed.");
            setTimeout(() => this.router.navigateByUrl('dashboard'), 3500);
          }
        }
      );
    }
  }
}
