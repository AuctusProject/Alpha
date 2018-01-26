import { Component, OnInit, Input } from '@angular/core';
import { PortfolioHistory } from "../../../model/portfolio/portfolioHistory";
import { HistoryValues } from "../../../model/advisor/historyValues";
import { PortfolioService } from "../../../services/portfolio.service";

@Component({
  selector: 'historical-tab',
  templateUrl: './historical-tab.component.html',
  styleUrls: ['./historical-tab.component.css']
})
export class HistoricalTabComponent implements OnInit {
  portfoliosHistoryModel: PortfolioHistory[];

  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.portfolioService.getPortfoliosHistory().subscribe(
      portfoliosHistory => {
        if (portfoliosHistory != undefined) {
          this.portfoliosHistoryModel = portfoliosHistory;
        }
      }
    );
  }
}
