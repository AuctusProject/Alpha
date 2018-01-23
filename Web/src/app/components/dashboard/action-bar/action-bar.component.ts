import { Component, OnInit, Input } from '@angular/core';
import { Projections } from "../../../model/portfolio/projections";
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";
import { PortfolioHistory } from "../../../model/portfolio/portfolioHistory";

@Component({
  selector: 'action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.css']
})
export class ActionBarComponent implements OnInit {
  @Input() projectionsModel: Projections;
  @Input() portfolioDistributionModel: PortfolioDistribution;
  @Input() portfolioHistoryModel: PortfolioHistory;

  constructor() { }

  ngOnInit() {
  }

}
