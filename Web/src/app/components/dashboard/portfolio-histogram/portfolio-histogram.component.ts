import { Component, OnInit, Input } from '@angular/core';
import { PortfolioHistory } from "../../../model/portfolio/portfolioHistory";
import { HistoryValues } from "../../../model/advisor/historyValues";
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'portfolio-histogram',
  templateUrl: './portfolio-histogram.component.html',
  styleUrls: ['./portfolio-histogram.component.css']
})
export class PortfolioHistogramComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
