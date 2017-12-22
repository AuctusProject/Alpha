import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advice/advisor";
import { Portfolio } from "../model/advice/portfolio";

@Injectable()
export class AdviceService {

  private listAdvisorsUrl = this.httpService.apiUrl("advice/advisors");
  private getAdvisorPortfoliosUrl = this.httpService.apiUrl("advice/advisors");

  constructor(private httpService : HttpService) { }

  getAdvisors(): Observable<Advisor[]> {
    return this.httpService.get(this.listAdvisorsUrl);
  }

  getAdvisorPortfolios(advisorId : number): Observable<Portfolio[]> {
    return this.httpService.get(this.getAdvisorPortfoliosUrl + "/" + advisorId + "/portfolios");
  }
}
