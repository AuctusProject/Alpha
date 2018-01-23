import { Component, OnInit, Input } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Projections } from "../../../model/portfolio/projections";

@Component({
  selector: 'app-projection-tab',
  templateUrl: './projection-tab.component.html',
  styleUrls: ['./projection-tab.component.css']
})
export class ProjectionTabComponent implements OnInit {
  projections: Projections;

  constructor(private portfolioService: PortfolioService) {
    this.projections = new Projections();
  }

  ngOnInit() {
    this.portfolioService.getProjections().subscribe(
      projections => {
        if (projections != undefined)
          this.projections = projections
      }
    )
  }
}
