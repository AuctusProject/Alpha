import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";
import { Goal } from "../../../model/account/goal";
import { ListRoboAdvisorsResponse } from "../../../model/portfolio/listRoboAdvisorsResponse";


@Component({
  selector: 'app-robo-advisors',
  templateUrl: './robo-advisors.component.html',
  styleUrls: ['./robo-advisors.component.css']
})
export class RoboAdvisorsComponent implements OnInit {
  portfolios: Portfolio[];
  userRisk: number;
  goal: Goal;


  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
  }

  onWizardSubmitted(goal: Goal) {
    this.goal = goal;
    this.getPortfolios();
  }

  private getPortfolios() {
    this.portfolioService.getRoboPortfolios(this.goal.goalOption.id, this.goal.risk).subscribe(
      listRoboAdvisorsResponse => {
        if (listRoboAdvisorsResponse) {
          this.portfolios = listRoboAdvisorsResponse.portfolios;
          this.userRisk = listRoboAdvisorsResponse.userRisk;
        }
      }
    );
  }

}
