import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advisor/advisor";

@Injectable()
export class AdvisorService {

  private baseGetAdvisorsUrl = this.httpService.apiUrl("advisors/v1");

  
  constructor(private httpService : HttpService) { }

  getAdvisors(): Observable<Advisor[]> {
    return this.httpService.get(this.baseGetAdvisorsUrl);
  }

  getAdvisorDetails(advisorId: number): Observable<Advisor> {
    return this.httpService.get(this.baseGetAdvisorsUrl + "/" + advisorId);
  }
}
