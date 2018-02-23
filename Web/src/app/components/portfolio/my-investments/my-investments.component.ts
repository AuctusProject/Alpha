import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";

@Component({
  selector: 'app-my-investments',
  templateUrl: './my-investments.component.html',
  styleUrls: ['./my-investments.component.css']
})
export class MyInvestmentsComponent implements OnInit {

  portfolios: Portfolio[];

  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.getPurchasedPortfolios();
  }

  private getPurchasedPortfolios() {
    this.portfolioService.getPurchasedPortfolios().subscribe(
      portfolios =>
        this.portfolios = portfolios);
  }

}
