import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-telegram-validator',
  templateUrl: './telegram-validator.component.html',
  styleUrls: ['./telegram-validator.component.css']
})
export class TelegramValidatorComponent implements OnInit {

  checkPromise: Subscription;
  constructor() { }

  ngOnInit() {
  }

  private checkParticipation() {

  }
}
