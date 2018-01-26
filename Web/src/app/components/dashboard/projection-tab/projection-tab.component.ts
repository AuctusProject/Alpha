import { Component, OnInit, Input } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Projections } from "../../../model/portfolio/projections";

@Component({
  selector: 'projection-tab',
  templateUrl: './projection-tab.component.html',
  styleUrls: ['./projection-tab.component.css']
})
export class ProjectionTabComponent implements OnInit {
  @Input() projections: Projections;

  constructor(private portfolioService: PortfolioService) {
  }

  ngOnInit() {
  }
}
