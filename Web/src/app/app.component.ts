import { Component, OnInit, NgZone } from '@angular/core';
import { Http } from '@angular/http'
import { Router } from '@angular/router'
import { NotificationsService } from "angular2-notifications";
import { EventsService } from "angular-event-service";
import { MetamaskAccountService } from "./services/metamask-account.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  public notificationOptions = {
    position: ["bottom", "left"],
    timeOut: 2500,
    maxStack: 1,
    preventDuplicates: true,
    preventLastDuplicates: "visible"
  }

  constructor(private _httpService: Http) {  
  }

  ngOnInit() {
  }
}
