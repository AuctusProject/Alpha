import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";
import { ListRoboAdvisorsResponse } from "../../../model/portfolio/listRoboAdvisorsResponse";


@Component({
  selector: 'app-robo-advisors',
  templateUrl: './robo-advisors.component.html',
  styleUrls: ['./robo-advisors.component.css']
})
export class RoboAdvisorsComponent implements OnInit {
  portfolios: Portfolio[];
  userRisk: number;

  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.getPortfolios();
  }

  private getPortfolios() {
    this.portfolioService.getRoboPortfolios(1, 3).subscribe(
      listRoboAdvisorsResponse => {
        if (listRoboAdvisorsResponse) {
          this.portfolios = listRoboAdvisorsResponse.portfolios;
          this.userRisk = listRoboAdvisorsResponse.userRisk;
        }
      }
    );
  }

}
