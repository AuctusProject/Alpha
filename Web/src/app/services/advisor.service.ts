import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advisor/advisor";
import { Portfolio } from "../model/portfolio/portfolio";

@Injectable()
export class AdvisorService {

  private baseGetAdvisorsUrl = this.httpService.apiUrl("advisors/v1");

  
  constructor(private httpService : HttpService) { }

  getAdvisor(id: string): Observable<Advisor> {
    return this.httpService.get(this.baseGetAdvisorsUrl + "/" + id);
  }

  createAdvisor(advisor: Advisor): Observable<Advisor> {
    return this.httpService.post(this.baseGetAdvisorsUrl, advisor);
  }

  updateAdvisor(advisor: Advisor): Observable<void> {
    return this.httpService.patch(this.baseGetAdvisorsUrl + "/" + advisor.id, advisor);
  }
}
