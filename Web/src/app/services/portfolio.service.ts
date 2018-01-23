import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Portfolio } from "../model/portfolio/portfolio";
import { Projections } from "../model/portfolio/projections";
import { PortfolioHistory } from "../model/portfolio/portfolioHistory";
import { PortfolioDistribution } from "../model/portfolio/portfolioDistribution";

@Injectable()
export class PortfolioService {
  
  private getProjectionsUrl = this.httpService.apiUrl("portfolios/v1/projections");
  private getPortfoliosHistoryUrl = this.httpService.apiUrl("portfolios/v1/history");
  private getPortfoliosDistributionUrl = this.httpService.apiUrl("portfolios/v1/distribution");


  constructor(private httpService: HttpService) { }

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
