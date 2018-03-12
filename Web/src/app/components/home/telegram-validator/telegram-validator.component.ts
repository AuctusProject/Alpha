import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { NotificationsService } from 'angular2-notifications';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-telegram-validator',
  templateUrl: './telegram-validator.component.html',
  styleUrls: ['./telegram-validator.component.css']
})
export class TelegramValidatorComponent implements OnInit {

  public phoneNumber: string;
  checkPromise: Subscription;


  constructor(private accountService: AccountService,
    private notificationService: NotificationsService) {
    
  }

  ngOnInit() {
  }

  private checkTelegramParticipation() {
    this.checkPromise = this.accountService.checkTelegram(this.phoneNumber).subscribe(result => {
      if (result) {
        this.notificationService.info("Info", result.isValid);
      }
    }, response => {
      this.notificationService.info("Error", response.error);
    });
  }
}
