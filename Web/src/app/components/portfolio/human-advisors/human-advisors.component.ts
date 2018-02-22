import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";

@Component({
  selector: 'app-human-advisors',
  templateUrl: './human-advisors.component.html',
  styleUrls: ['./human-advisors.component.css']
})
export class HumanAdvisorsComponent implements OnInit {

  portfolios: Portfolio[];/* = [{
    id: 33,
    name: "fsdf",
    description: "fgfgfd",
    price: 222.0,
    purchased: false,
    owned: false,
    enabled: true,
    purchaseQuantity: 0,
    advisorId: 36,
    advisorName: "rwer",
    advisorDescription: "werwer",
    risk: 1,
    projectionPercent: 222.0,
    optimisticPercent: null,
    pessimisticPercent: null,
    totalDays: 0,
    lastDay: null,
    last7Days: null,
    last30Days: null,
    allDays: {
      value: 37,
      expectedValue: 35,
      optimisticExpectation: 45,
      pessimisticExpectation: 30,
      hitPercentage: 75.5
    }
  },
  {
    id: 33,
    name: "Nome do Portfolio",
    description: "Descrição do Portfolio",
    price: 222.0,
    purchased: false,
    owned: false,
    enabled: true,
    purchaseQuantity: 0,
    advisorId: 36,
    advisorName: "Nome do Advisor",
    advisorDescription: "Descrição do Advisor",
    risk: 5,
    projectionPercent: 222.0,
    optimisticPercent: null,
    pessimisticPercent: null,
    totalDays: 0,
    lastDay: null,
    last7Days: null,
    last30Days: null,
    allDays: {
      value: 37,
      expectedValue: 35,
      optimisticExpectation: 45,
      pessimisticExpectation: 30,
      hitPercentage: 75.5
    }
  }];*/

  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.getPortfolios();
  }

  private getPortfolios() {
    this.portfolioService.getPortfolios().subscribe(
      portfolios =>
        this.portfolios = portfolios);
  }
}
