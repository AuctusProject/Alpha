import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Projections } from "../../../model/portfolio/projections";
import { Goal } from "../account/goal";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  projections: Projections;
  tabs: any;
  public selectedTab = 0;
  public currentGoal: Goal;

  constructor(private portfolioService: PortfolioService) {
    this.tabs = ['Projection', 'Portfolio', 'History'];
  }

  ngOnInit() {
    this.portfolioService.getProjections().subscribe(
      projections => {
        if (projections != undefined) {
          this.projections = projections
          this.currentGoal = projections.currentGoal;
        }
      }
    )
  }

  public onTabSelected(index: number) {
    this.selectedTab = index;
  }

  public shouldLoadTab(index: number) {
    return this.selectedTab == index || this.projections;
  }

}
