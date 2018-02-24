import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MetamaskAccountService } from "../../services/metamask-account.service";
import { LoginService } from "../../services/login.service";
import { EventsService } from "angular-event-service";

@Component({
  selector: 'metamask-account-monitor',
  templateUrl: './metamask-account-monitor.component.html',
  styleUrls: ['./metamask-account-monitor.component.css']
})
export class MetamaskAccountMonitorComponent implements OnInit {

  constructor(private metamaskAccount: MetamaskAccountService,
    private eventsService: EventsService,
    private router: Router,
    private zone: NgZone,
    private loginService: LoginService) { }

  ngOnInit() {
    this.eventsService.on("loginConditionsFail", this.onLoginConditionsFail);
    this.eventsService.on("loginConditionsSuccess", this.onLoginConditionsSuccess);
  }

  ngOnDestroy() {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.eventsService.destroyListener("loginConditionsFail", this.onLoginConditionsFail);
    this.eventsService.destroyListener("loginConditionsSuccess", this.onLoginConditionsSuccess);
  }

  private onLoginConditionsFail: Function = (payload: any) => {
    if (this.router.url != "/required") {
      this.loginService.logoutWithoutRedirect();
      this.zone.run(() => this.router.navigateByUrl('required'));
    }
  }

  private onLoginConditionsSuccess: Function = (payload: any) => {
    if (this.router.url == "/required") {
      this.zone.run(() => this.router.navigate(['home']));
    }
  }

}
