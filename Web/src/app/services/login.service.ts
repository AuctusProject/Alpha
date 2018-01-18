import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Login } from '../model/account/login';
import { LoginResult } from '../model/account/loginResult';
import { HttpService } from './http.service';

@Injectable()
export class LoginService {

  private loginUrl = this.httpService.apiUrl("account/login");

  constructor(private httpService : HttpService)
  { }

  login(login: Login): Observable<LoginResult> {
    return this.httpService.post(this.loginUrl, login);
  }
}
