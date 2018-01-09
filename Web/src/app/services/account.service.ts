import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { GoalOption } from "../model/goalOption";
import { FullRegister } from "../model/account/fullRegister";

@Injectable()
export class AccountService {

  private listGoalOptionsUrl = this.httpService.apiUrl("account/goaloptions");
  private fullRegisterUrl = this.httpService.apiUrl("account/fullregister");
  private confirmEmailUrl = this.httpService.apiUrl("account/confirm");

  constructor(private httpService : HttpService) { }

  listGoalOptions(): Observable<GoalOption[]> {
    return this.httpService.get(this.listGoalOptionsUrl);
  }

  fullRegister(fullRegisterDTO : FullRegister): Observable<FullRegister> {
    return this.httpService.post(this.fullRegisterUrl, fullRegisterDTO);
  }

  confirm(code : String, email: String): Observable<Boolean> {
    let json = {
      code : code,
      email: email
    };
    return this.httpService.post(this.confirmEmailUrl, json);
  }
}
