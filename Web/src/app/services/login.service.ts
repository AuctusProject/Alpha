import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Login } from '../model/account/login';
import { HttpService } from './http.service';

@Injectable()
export class LoginService {

  private loginUrl = this.httpService.apiUrl("account/login");

  constructor(private httpService : HttpService)
  { }

  login(login: Login): Observable<string> {
    return this.httpService.post(this.loginUrl, login);
  }
}
