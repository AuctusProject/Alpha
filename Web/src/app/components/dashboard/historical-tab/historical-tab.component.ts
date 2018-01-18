import { Component, OnInit, Input } from '@angular/core';
import { PortfolioHistory } from "../../../model/advice/portfolioHistory";
import { AdviceService } from "../../../services/advice.service";

@Component({
  selector: 'app-historical-tab',
  templateUrl: './historical-tab.component.html',
  styleUrls: ['./historical-tab.component.css']
})
export class HistoricalTabComponent implements OnInit {
  @Input() portfoliosHistoryModel: PortfolioHistory[];
  portfolioHistory: PortfolioHistory;

  constructor(private adviceService: AdviceService) { }

  ngOnInit() {
    this.adviceService.getPortfoliosHistory().subscribe(
      portfoliosHistory => {
        if (portfoliosHistory != undefined) {
          this.portfoliosHistoryModel = portfoliosHistory;
          this.portfolioHistory = portfoliosHistory[0];
        }
      }
    );
  }

}
