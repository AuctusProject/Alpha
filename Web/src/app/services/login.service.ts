import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Login } from '../model/account/login';
import { LoginResult } from '../model/account/loginResult';
import { HttpService } from './http.service';
import { Router } from '@angular/router';
import { LocalStorageService } from './local-storage.service';

@Injectable()
export class LoginService {

  private loginUrl = this.httpService.apiUrl("accounts/v1/login");

  constructor(private httpService: HttpService, 
    private router: Router, 
    private localStorageService: LocalStorageService)
  { }

  login(login: Login): Observable<LoginResult> {
    return this.httpService.post(this.loginUrl, login);
  }

  setLoginData(loginData: string): void {
    this.httpService.setLoginData(loginData);
  }

  getLoginData(): any {
    return this.httpService.getLoginData();
  }

  getUser(): string {
    return this.httpService.getUser();
  }

  logout(): void {
    this.httpService.logout();
    this.router.navigateByUrl('login');
  }

  logoutWithoutRedirect(): void {
    this.httpService.logout();
  }

  isLoggedIn() : boolean{
    return this.httpService.isLoggedIn();
  }

  setLoginRedirectUrl(urlToRedirect): void{
    this.localStorageService.setLocalStorage('loginRedirectUrl',urlToRedirect)
  }

  getLoginRedirectUrl(): string{
    return this.localStorageService.getLocalStorage('loginRedirectUrl');
  }
}
