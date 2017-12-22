import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advice/advisor";

@Injectable()
export class AdviceService {

  private listAdvisors = this.httpService.apiUrl("advice/advisors");

  constructor(private httpService : HttpService) { }

  getAdvisors(): Observable<Advisor[]> {
    return this.httpService.get(this.listAdvisors);
  }
}
