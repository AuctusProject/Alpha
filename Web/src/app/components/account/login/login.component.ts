import { Component, OnInit, Input } from '@angular/core';
import { Login } from '../../../model/account/login';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Input() login: Login = new Login();

  constructor(private loginService: LoginService) { }

  ngOnInit() {
    
  } 

  onLoginClick(): void {
    console.log(this.login);
    this.loginService.login(this.login)
      .subscribe(response => {
        console.log(response);
      });
  }

}
