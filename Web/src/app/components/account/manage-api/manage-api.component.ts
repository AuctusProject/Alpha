import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { NotificationsService } from 'angular2-notifications';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-manage-api',
  templateUrl: './manage-api.component.html',
  styleUrls: ['./manage-api.component.css']
})
export class ManageApiComponent implements OnInit {

  @Input() apiKey: string = null;
  promise: Subscription;

  constructor(private accountService: AccountService, private notificationService: NotificationsService) { }

  ngOnInit() {
    this.accountService.getLastApiKey().subscribe(
      response => {
        if (response && response.key) {
          this.apiKey = response.key;
        }
      }
    );
  }

  onGenerateApiKeyClick(): void {
    this.promise = this.accountService.generateApiKey().subscribe(
      response => {
        if (response && response.key) {
          this.apiKey = response.key;
          this.notificationService.success("Sucess", "A new API key was generated.");
        }
      }
    );
  }

  onRevokeClick(): void {
    this.promise = this.accountService.revokeApiKey().subscribe(
      response => {
        if (response) {
          this.apiKey = null;
          this.notificationService.success("Sucess", "API key was revoked.");
        }
      }
    );
  }
}
