import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { PortfolioService } from '../../../services/portfolio.service';
import { ActivatedRoute, Params } from '@angular/router';
import { LocalStorageService } from "../../../services/local-storage.service";
import { Goal } from '../../../model/account/goal';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'app-portfolio-details',
  templateUrl: './portfolio-details.component.html',
  styleUrls: ['./portfolio-details.component.css']
})
export class PortfolioDetailsComponent implements OnInit {

  public portfolio: Portfolio;
  public goal: Goal;
  loginData: any;

  private readonly roboAdvisorType: number = 1;

  constructor(private portfolioService: PortfolioService,
    private activatedRoute: ActivatedRoute,
    private localStorageService: LocalStorageService,
    private loginService: LoginService) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.getPortfolio(params['id']);
    });

    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.loginData = this.loginService.getLoginData();
    }
  }

  private getPortfolio(portfolioId){
    this.portfolioService.getPortfolio(portfolioId)
    .subscribe(response => {
      this.portfolio = response;
      this.setGoal();
    })
  }

  private setGoal(){
    if (this.portfolio.advisorType == this.roboAdvisorType && this.localStorageService.getLocalStorage("currentGoal")){
      this.goal = JSON.parse(this.localStorageService.getLocalStorage("currentGoal"));
    }
    this.localStorageService.removeLocalStorage("currentGoal");
  }

  private getRiskDescription(risk) {
    if (risk == 1) {
      return "Conservative";
    }
    else if (risk == 2) {
      return "Moderately Conservative";
    }
    else if (risk == 3) {
      return "Moderately Aggressive";
    }
    else if (risk == 4) {
      return "Aggressive";
    }
    else if (risk == 5) {
      return "Very Aggressive"
    }
  }

}
