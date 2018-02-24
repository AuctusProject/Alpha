import { Component, OnInit, Input } from '@angular/core';
import { PortfolioRequest } from '../../../model/portfolio/portfolioRequest';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { LoginService } from '../../../services/login.service';
import { PortfolioService } from '../../../services/portfolio.service';
import { Asset } from '../../../model/asset/asset';
import { PublicService } from '../../../services/public.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { AssetDistribution } from '../../../model/asset/assetDistribution'

@Component({
  selector: 'app-add-portfolio',
  templateUrl: './add-portfolio.component.html',
  styleUrls: ['./add-portfolio.component.css']
})
export class AddPortfolioComponent implements OnInit {
  portfolioId: number
  assetsDistributionRows: AssetDistribution[];

  portfolio: PortfolioRequest;
  loginData: any;
  assets: Asset[];

  constructor(private loginService: LoginService, private publicService: PublicService, private portfolioService: PortfolioService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getLoginData();
    this.route.params.subscribe(params => this.initComponent(params));
  }

  initComponent(params: any) {
    if (params && params['id']) {
      this.portfolioId = params['id'];
    }
    this.createPortfolioRequest();
  }

  getLoginData() {
    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.loginData = this.loginService.getLoginData();
      if (this.loginData) {
        return;
      }
    }
    this.loginService.logout();
  }

  createPortfolioRequest() {
    if (this.portfolioId) {
      this.portfolioService.getPortfolio(this.portfolioId).subscribe(portfolio =>
        this.fillPortfolioRequestWithPortfolioModel(portfolio)
      );
    }
    else {
      this.portfolio = new PortfolioRequest();
      this.portfolio.advisorId = this.loginData.humanAdvisorId;
      this.portfolio.isEditing = true;
    }
    this.publicService.listAssets().subscribe(assets => this.assets = assets);
  }

  fillPortfolioRequestWithPortfolioModel(portfolio: Portfolio) {
    if (this.loginData.humanAdvisorId != portfolio.advisorId) {
      this.router.navigateByUrl('advisor/' + this.loginData.humanAdvisorId);
    }
    this.portfolio = new PortfolioRequest();
    this.portfolio.isEditing = true;
    this.portfolio.advisorId = portfolio.advisorId;
    this.portfolio.description = portfolio.description;
    this.portfolio.distribution = [];
    this.assetsDistributionRows = portfolio.assetDistribution;
    for (let assetDistribution of portfolio.assetDistribution) {
      this.portfolio.distribution.push({ assetId: assetDistribution.id, percentage: assetDistribution.percentage });
    }
    this.portfolio.id = portfolio.id;
    this.portfolio.name = portfolio.name;
    this.portfolio.optimisticProjection = portfolio.optimisticPercent;
    this.portfolio.pessimisticProjection = portfolio.pessimisticPercent;
    this.portfolio.price = portfolio.price;
    this.portfolio.projectionValue = portfolio.projectionPercent;
    this.portfolio.risk = portfolio.risk;
  }

  public onPortfolioSaved() {
    this.router.navigateByUrl('advisor/' + this.loginData.humanAdvisorId);
  }
}
