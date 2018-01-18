import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advice/advisor";
import { Portfolio } from "../model/advice/portfolio";
import { Projections } from "../model/advice/projections";
import { PortfolioHistory } from "../model/advice/portfolioHistory";
import { PortfolioDistribution } from "../model/advice/portfolioDistribution";

@Injectable()
export class AdviceService {

  private listAdvisorsUrl = this.httpService.apiUrl("advice/advisors");
  private getAdvisorPortfoliosUrl = this.httpService.apiUrl("advice/advisors");
  private getProjectionsUrl = this.httpService.apiUrl("advice/projections");
  private getPortfoliosHistoryUrl = this.httpService.apiUrl("advice/portfolio/history");
  private getPortfoliosDistributionUrl = this.httpService.apiUrl("advice/portfolio/distribution");

  
  constructor(private httpService : HttpService) { }

  getAdvisors(): Observable<Advisor[]> {
    return this.httpService.get(this.listAdvisorsUrl);
  }

  getAdvisorDetails(advisorId : number): Observable<Portfolio[]> {
    return this.httpService.get(this.getAdvisorPortfoliosUrl + "/" + advisorId + "/details");
  }

  getProjections(): Observable<Projections> {
    return this.httpService.get(this.getProjectionsUrl);
  }

  getPortfoliosHistory(): Observable<PortfolioHistory[]> {
    return this.httpService.get(this.getPortfoliosHistoryUrl);
  }

  getPortfoliosDistribution(): Observable<PortfolioDistribution[]> {
    return this.httpService.get(this.getPortfoliosDistributionUrl);
  }
}
