import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Portfolio } from "../model/portfolio/portfolio";
import { PortfolioRequest } from "../model/portfolio/portfolioRequest";
import { Projections } from "../model/portfolio/projections";
import { PortfolioHistory } from "../model/portfolio/portfolioHistory";
import { PortfolioDistribution } from "../model/portfolio/portfolioDistribution";
import { ListRoboAdvisorsResponse } from "../model/portfolio/listRoboAdvisorsResponse";

@Injectable()
export class PortfolioService {
  
  private getProjectionsUrl = this.httpService.apiUrl("portfolios/v1/projections");
  private getPortfoliosHistoryUrl = this.httpService.apiUrl("portfolios/v1/history");
  private getPortfoliosDistributionUrl = this.httpService.apiUrl("portfolios/v1/distribution");
  private savePortfolioUrl = this.httpService.apiUrl("portfolios/v1/");
  private getPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/");
  private getRoboPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/robos");
  private getPurchasedPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/purchases");
  
  constructor(private httpService: HttpService) { }

  getProjections(): Observable<Projections> {
    return this.httpService.get(this.getProjectionsUrl);
  }

  getPortfolios(): Observable<Portfolio[]> {
    return this.httpService.get(this.getPortfoliosUrl);
  }

  getPortfolio(portfolioId): Observable<Portfolio> {
    return this.httpService.get(this.getPortfoliosUrl + portfolioId);
  }

  getPurchasedPortfolios(): Observable<Portfolio[]> {
    return this.httpService.get(this.getPurchasedPortfoliosUrl);
  }

  getPortfoliosHistory(): Observable<PortfolioHistory[]> {
    return this.httpService.get(this.getPortfoliosHistoryUrl);
  }

  getPortfoliosDistribution(): Observable<PortfolioDistribution[]> {
    return this.httpService.get(this.getPortfoliosDistributionUrl);
  }

  createPortfolio(portfolioRequest: PortfolioRequest): Observable<PortfolioRequest> {
    return this.httpService.post(this.savePortfolioUrl, portfolioRequest);
  }

  updatePortfolio(portfolioRequest: PortfolioRequest): Observable<PortfolioRequest> {
    return this.httpService.put(this.savePortfolioUrl, portfolioRequest);
  }

  getRoboPortfolios(goalOption: number, risk: number): Observable<ListRoboAdvisorsResponse> {
    return this.httpService.get(this.getRoboPortfoliosUrl + "?goalOption=" + goalOption + "&risk=" + risk);
  }
}
