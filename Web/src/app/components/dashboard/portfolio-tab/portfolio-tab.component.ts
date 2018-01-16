import { Component, OnInit, Input } from '@angular/core';
import { PortfolioDistribution } from "../../../model/advice/portfolioDistribution";

@Component({
  selector: 'app-portfolio-tab',
  templateUrl: './portfolio-tab.component.html',
  styleUrls: ['./portfolio-tab.component.css']
})
export class PortfolioTabComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution;
  
  constructor() { }

  ngOnInit() {
  }

}
