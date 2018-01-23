import { Component, OnInit, Input } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { PortfolioDistribution } from "../../../model/advice/portfolioDistribution";

@Component({
  selector: 'app-portfolio-tab',
  templateUrl: './portfolio-tab.component.html',
  styleUrls: ['./portfolio-tab.component.css']
})
export class PortfolioTabComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution[];
  portfolioDistribution: PortfolioDistribution;

  constructor(private adviceService: AdviceService) {
    this.portfolioDistributionModel = [];
  }

  ngOnInit() {
    this.adviceService.getPortfoliosDistribution().subscribe(
      portfoliosDistribution => {
        if (portfoliosDistribution != undefined) {
          this.portfolioDistributionModel = portfoliosDistribution;
          this.portfolioDistribution = portfoliosDistribution[0];
        }
      }
    )
  }
}
