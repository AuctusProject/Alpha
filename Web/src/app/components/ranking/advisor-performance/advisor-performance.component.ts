
import { Component, OnInit } from '@angular/core';
import { AdvisorRank } from '../../../model/advisor/advisorRank';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { PortfolioService } from '../../../services/portfolio.service';
import { AdvisorService } from '../../../services/advisor.service'

@Component({
  selector: 'app-advisor-performance',
  templateUrl: './advisor-performance.component.html',
  styleUrls: ['./advisor-performance.component.css']
})
export class AdvisorPerformanceComponent implements OnInit {
  portfolios: Portfolio[];
  advisors: AdvisorRank[];
  dailyPortfolios: Portfolio[];
  rankingDate: Date;
  public maxDate: Date;

  constructor(private portfolioService: PortfolioService,
    private advisorService: AdvisorService) {
    this.maxDate = new Date();
    this.maxDate.setDate(this.maxDate.getDate()-1);
  }

  ngOnInit() {
    this.rankingDate = new Date();
    this.rankingDate.setDate(this.rankingDate.getDate() - 1);
    this.getPortfoliosByDate();
    this.getAllPortfolios();
    this.getAdvisorsRank();
  }

  private getPortfoliosByDate() {
    this.dailyPortfolios = null;
    this.portfolioService.getPortfoliosPerformance(this.rankingDate).subscribe(
      portfolios =>
        this.dailyPortfolios = portfolios);
  }

  private getAllPortfolios() {
    this.portfolioService.getAllPortfoliosPerformance().subscribe(
      portfolios =>
        this.portfolios = portfolios);
  }

  private getAdvisorsRank() {
    this.advisorService.getAdvisorsRank().subscribe(
      advisorList =>
       this.advisors = advisorList);
  }

  onRankingDateChange(){
    this.getPortfoliosByDate();
  }

}
