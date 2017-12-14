import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';

@Injectable()
export class AccountService {

  private listGoalOptionsUrl = this.httpService.apiUrl("account/goaloptions");

  constructor(private httpService : HttpService) { }

  listGoalOptions(): Observable<string> {
    return this.httpService.get(this.listGoalOptionsUrl);
  }
}
