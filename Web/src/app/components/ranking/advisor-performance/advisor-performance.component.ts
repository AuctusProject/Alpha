import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { PortfolioService } from '../../../services/portfolio.service';

@Component({
  selector: 'app-advisor-performance',
  templateUrl: './advisor-performance.component.html',
  styleUrls: ['./advisor-performance.component.css']
})
export class AdvisorPerformanceComponent implements OnInit {
  portfolios: Portfolio[];
  dailyPortfolios: Portfolio[];
  rankingDate: Date;
  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.rankingDate = new Date();
    this.rankingDate.setDate(this.rankingDate.getDate() - 1);
    this.getPortfoliosByDate();
    this.getAllPortfolios();
  }

  private getPortfoliosByDate() {
    this.dailyPortfolios = null;
    this.portfolioService.getPortfoliosPerformance(this.rankingDate).subscribe(
      portfolios =>
        this.dailyPortfolios = portfolios);
  }

  private getAllPortfolios() {
    this.portfolioService.getPortfolios().subscribe(
      portfolios =>
        this.portfolios = portfolios);
  }

  onRankingDateChange(){
    this.getPortfoliosByDate();
  }

}
