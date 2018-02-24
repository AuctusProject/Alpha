import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  loginData: any;


  constructor(private loginService: LoginService) { }

  ngOnInit() {
    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.loginData = this.loginService.getLoginData();
    }
  }

  logout(){
    this.loginService.logout();
  }

  getUsernameFirstLetter(){
    if(this.loginData && this.loginData.username && this.loginData.username.length > 0)
      return this.loginData.username[0].toUpperCase();
    return "-";
  }
}
