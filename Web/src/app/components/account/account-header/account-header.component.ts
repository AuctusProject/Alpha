import { Component, OnInit, Input } from '@angular/core';
import { LoginService } from '../../../services/login.service';
import { MediaChange, ObservableMedia } from "@angular/flex-layout";

@Component({
  selector: 'account-header',
  templateUrl: './account-header.component.html',
  styleUrls: ['./account-header.component.css']
})
export class AccountHeaderComponent implements OnInit {

  @Input() changePassword: boolean = false;

  constructor(private loginService: LoginService, public media: ObservableMedia) { }

  ngOnInit() {
  }

  logout(): void {
    this.loginService.logout();
  }
}
