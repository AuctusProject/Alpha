import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Portfolio } from "../model/portfolio/portfolio";
import { PortfolioRequest } from "../model/portfolio/portfolioRequest";
import { PortfolioUpdateRequest } from "../model/portfolio/portfolioUpdateRequest";
import { Projections } from "../model/portfolio/projections";
import { PortfolioHistory } from "../model/portfolio/portfolioHistory";
import { PortfolioDistribution } from "../model/portfolio/portfolioDistribution";
import { ListRoboAdvisorsResponse } from "../model/portfolio/listRoboAdvisorsResponse";
import { Investments } from '../model/portfolio/investments';
import { ExchangePortfolio } from '../model/portfolio/exchangePortfolio';
import { FollowPortfolio } from '../model/portfolio/followPortfolio';

@Injectable()
export class PortfolioService {
  
  private getProjectionsUrl = this.httpService.apiUrl("portfolios/v1/projections");
  private getPortfoliosHistoryUrl = this.httpService.apiUrl("portfolios/v1/history");
  private getPortfoliosDistributionUrl = this.httpService.apiUrl("portfolios/v1/distribution");
  private savePortfolioUrl = this.httpService.apiUrl("portfolios/v1/");
  private getPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/");
  private getRoboPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/robos");
  private getPurchasedPortfoliosUrl = this.httpService.apiUrl("portfolios/v1/purchases");
  private getInvestmentsUrl = this.httpService.apiUrl("portfolios/v1/investments");
  private getExchangePortfolioUrl = this.httpService.apiUrl("portfolios/v1/exchange/");
  private deleteExchangePortfolioUrl = this.httpService.apiUrl("portfolios/v1/exchange/");
  private followersUrl = this.httpService.apiUrl("portfolios/v1/followers/");
  private unfollowUrl = this.httpService.apiUrl("portfolios/v1/followers/unfollow");
  
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

  getInvestments(): Observable<Investments> {
    return this.httpService.get(this.getInvestmentsUrl);
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

  updatePortfolio(portfolioId:number, portfolioRequest: PortfolioUpdateRequest): Observable<PortfolioRequest> {
    return this.httpService.put(this.savePortfolioUrl + portfolioId, portfolioRequest);
  }

  getRoboPortfolios(goalOption: number, risk: number): Observable<ListRoboAdvisorsResponse> {
    return this.httpService.get(this.getRoboPortfoliosUrl + "?goalOption=" + goalOption + "&risk=" + risk);
  }

  getExchangePortfolio(exchangeId: number): Observable<ExchangePortfolio> {
    return this.httpService.get(this.getExchangePortfolioUrl + exchangeId);
  }

  deleteExchangePortfolio(exchangeId: number): Observable<void> {
    return this.httpService.delete(this.getExchangePortfolioUrl + exchangeId);
  }

  followPortfolio(data: FollowPortfolio): Observable<void> {
    return this.httpService.post(this.followersUrl, data);
  }

  unfollowPortfolio(data: FollowPortfolio): Observable<void> {
    return this.httpService.post(this.unfollowUrl, data);
  }  
}
