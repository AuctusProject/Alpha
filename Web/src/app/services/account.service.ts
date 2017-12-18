import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { GoalOption } from "../model/goalOption";

@Injectable()
export class AccountService {

  private listGoalOptionsUrl = this.httpService.apiUrl("account/goaloptions");

  constructor(private httpService : HttpService) { }

  listGoalOptions(): Observable<GoalOption[]> {
    return this.httpService.get(this.listGoalOptionsUrl);
  }
}
