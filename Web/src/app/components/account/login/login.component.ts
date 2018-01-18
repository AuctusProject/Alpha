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
    
  } 

  onLoginClick(): void {
    console.log(this.login);
    this.loginService.login(this.login)
      .subscribe(response => {
        if (response.logged) {
          this.router.navigateByUrl('dashboard');
        }
        else {
          this.notificationService.error("Error", response.error);
        }
        console.log(response);
      });
  }

}
