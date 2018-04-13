import { Component, OnInit } from "@angular/core";
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";
import { ExchangePortfolio } from "../../../model/portfolio/exchangePortfolio";

@Component({
  selector: "app-my-investments",
  templateUrl: "./my-investments.component.html",
  styleUrls: ["./my-investments.component.css"]
})
export class MyInvestmentsComponent implements OnInit {
  portfolios: Portfolio[];
  exchangePortfolios: ExchangePortfolio[];

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit() {
    this.getPurchasedPortfolios();
  }

  private getPurchasedPortfolios() {
    this.portfolioService.getInvestments().subscribe(investments => {
      if(investments){
        this.portfolios = investments.purchasedPortfolios;
        this.exchangePortfolios = investments.exchangePortfolios;
      }
    });
  }
}
