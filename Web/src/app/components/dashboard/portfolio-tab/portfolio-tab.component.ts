import { Component, OnInit, Input } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";

@Component({
  selector: 'portfolio-tab',
  templateUrl: './portfolio-tab.component.html',
  styleUrls: ['./portfolio-tab.component.css']
})
export class PortfolioTabComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution[];
  portfolioDistribution: PortfolioDistribution;

  constructor(private portfolioService: PortfolioService) {
    this.portfolioDistributionModel = [];
  }

  ngOnInit() {
    this.portfolioService.getPortfoliosDistribution().subscribe(
      portfoliosDistribution => {
        if (portfoliosDistribution != undefined) {
          this.portfolioDistributionModel = portfoliosDistribution;
          this.portfolioDistribution = portfoliosDistribution[0];
        }
      }
    )
  }
}
