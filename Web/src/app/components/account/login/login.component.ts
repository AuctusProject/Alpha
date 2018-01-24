import { Component, OnInit, Input } from '@angular/core';
import { Login } from '../../../model/account/login';
import { LoginResult } from '../../../model/account/loginResult';
import { LoginService } from '../../../services/login.service';
import { NotificationsService } from "angular2-notifications";
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Input() login: Login = new Login();

  constructor(private loginService: LoginService, private notificationService: NotificationsService, private router: Router) { }

  ngOnInit() {
    this.login.pendingConfirmation = false;
  } 

  onLoginClick(): void {
    this.loginService.login(this.login)
      .subscribe(response => {
        if (response.logged) {
          this.router.navigateByUrl('dashboard');
        }
        else {
          this.login.pendingConfirmation = true;
          this.notificationService.info("Info", response.error);
        }
      });
  }
}
