import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from "angular2-notifications";
import { Router } from '@angular/router';

@Component({
  selector: 'app-pending-email-confirmation-component',
  templateUrl: './pending-email-confirmation.component.html',
  styleUrls: ['./pending-email-confirmation.component.css']
})
export class PendingEmailConfirmationComponent implements OnInit {

  @Input() emailSent: boolean;
  @Input() email: string;

  constructor(private accountService: AccountService, private notificationService: NotificationsService, private router: Router) { }

  ngOnInit() {
    this.emailSent = false;
  }

  onResendEmailClick(): void {
    this.accountService.resendConfirmation(this.email).subscribe(response => {
      if (response) {
        this.emailSent = true;
        this.notificationService.info("Verify you mail box", "Follow email instructions.");
      }
    });
  }
}
