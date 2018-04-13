import { Component, OnInit } from '@angular/core';
import { ExchangeApiAccessRequest } from '../../../model/account/exchangeApiAccessRequest';
import { AccountService } from '../../../services/account.service';
import { Subscription } from 'rxjs';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-sync-exchange',
  templateUrl: './sync-exchange.component.html',
  styleUrls: ['./sync-exchange.component.css']
})
export class SyncExchangeComponent implements OnInit {
  model: ExchangeApiAccessRequest = new ExchangeApiAccessRequest(); 
  syncPromise: Subscription;
  constructor(private accountService: AccountService) { }

  ngOnInit() {
  }

  saveClick(){
    this.syncPromise = this.accountService.saveExchangeApiAccess(this.model).subscribe();
  }

}
