import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Login } from '../model/account/login';
import { LoginResult } from '../model/account/loginResult';
import { HttpService } from './http.service';
import { Router } from '@angular/router';

@Injectable()
export class LoginService {

  private loginUrl = this.httpService.apiUrl("accounts/v1/login");

  constructor(private httpService: HttpService, private router: Router)
  { }

  login(login: Login): Observable<LoginResult> {
    return this.httpService.post(this.loginUrl, login);
  }

  setUser(email: string): void {
    this.httpService.setUser(email);
  }

  getUser(): string {
    return this.httpService.getUser();
  }

  logout(): void {
    this.httpService.logout();
    this.router.navigateByUrl('home');
  }

  isLoggedIn() : boolean{
    return this.httpService.isLoggedIn();
  }
}
