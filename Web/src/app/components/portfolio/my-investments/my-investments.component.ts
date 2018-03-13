import { Component, OnInit } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'app-my-investments',
  templateUrl: './my-investments.component.html',
  styleUrls: ['./my-investments.component.css']
})
export class MyInvestmentsComponent implements OnInit {

  portfolios: Portfolio[];

  constructor(private portfolioService: PortfolioService, private loginService: LoginService) { }

  ngOnInit() {
    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.getPurchasedPortfolios();
    }
  }

  private getPurchasedPortfolios() {
    this.portfolioService.getPurchasedPortfolios().subscribe(
      portfolios =>
        this.portfolios = portfolios);
  }

}
