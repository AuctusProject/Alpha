import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from "angular2-notifications";
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-pending-email-confirmation-component',
  templateUrl: './pending-email-confirmation.component.html',
  styleUrls: ['./pending-email-confirmation.component.css']
})
export class PendingEmailConfirmationComponent implements OnInit {

  @Input() emailSent: boolean;
  @Input() email: string;
  promise: Subscription;

  constructor(private accountService: AccountService, private notificationService: NotificationsService) { }

  ngOnInit() {
    this.emailSent = false;
  }

  onResendEmailClick(): void {
    this.promise = this.accountService.resendConfirmation(this.email).subscribe(response => {
      if (response) {
        this.emailSent = true;
        this.notificationService.info("Verify you mail box", "Follow email instructions.");
      }
    });
  }
}
