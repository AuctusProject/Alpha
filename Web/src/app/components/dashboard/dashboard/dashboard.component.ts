import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Projections } from "../../../model/portfolio/projections";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  projections: Projections;

  constructor(private portfolioService: PortfolioService) {
    this.projections = new Projections();
  }

  ngOnInit() {
    this.portfolioService.getProjections().subscribe(
      projections => {
        if (projections!=undefined)
          this.projections = projections
      }
    )
  }
}
