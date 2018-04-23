import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { PortfolioService } from '../../../services/portfolio.service';
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'app-exchange-portfolio-details',
  templateUrl: './exchange-portfolio-details.component.html',
  styleUrls: ['./exchange-portfolio-details.component.css']
})
export class ExchangePortfolioDetailsComponent implements OnInit {

  constructor(private route: ActivatedRoute, private portfolioService: PortfolioService) { }
  portfolio: Portfolio;

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.getExchangePortfolio(params['id']);
    });
  }

  getExchangePortfolio(exchangeId){
    this.portfolioService.getExchangePortfolio(exchangeId).subscribe(portfolio => this.portfolio)
  }
}
