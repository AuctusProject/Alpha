import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { MetamaskAccountService } from "../../services/metamask-account.service";
import { EventsService } from "angular-event-service";

@Component({
  selector: 'metamask-account-monitor',
  templateUrl: './metamask-account-monitor.component.html',
  styleUrls: ['./metamask-account-monitor.component.css']
})
export class MetamaskAccountMonitorComponent implements OnInit {

  constructor(private metamaskAccount : MetamaskAccountService,
    private eventsService: EventsService,
    private router: Router, 
    private zone: NgZone) { }

  ngOnInit() {
    this.eventsService.on("loginConditionsFail", this.onLoginConditionsFail);
  }

  private onLoginConditionsFail: Function = (payload: any) => {
    if (this.router.url != "/required"){
      this.zone.run(() => this.router.navigateByUrl('required'));
    }
  }

}
