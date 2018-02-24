import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";

@Component({
  selector: 'app-human-advisors',
  templateUrl: './human-advisors.component.html',
  styleUrls: ['./human-advisors.component.css']
})
export class HumanAdvisorsComponent implements OnInit {

  portfolios: Portfolio[];

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
