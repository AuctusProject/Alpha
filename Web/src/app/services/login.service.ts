import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Login } from '../model/login';
import { HttpService } from './http.service';

@Injectable()
export class LoginService {

  private loginUrl = "http://localhost:16814/api/login"

  constructor(private httpService : HttpService)
  { }

  login(login: Login): Observable<string> {
    return this.httpService.post(this.loginUrl, login);
  }
}
