import { Component, OnInit, Input } from '@angular/core';
import { PortfolioHistory } from "../../../model/advice/portfolioHistory";

@Component({
  selector: 'app-historical-tab',
  templateUrl: './historical-tab.component.html',
  styleUrls: ['./historical-tab.component.css']
})
export class HistoricalTabComponent implements OnInit {
  @Input() portfolioHistoryModel: PortfolioHistory;

  constructor() { }

  ngOnInit() {
  }

}
