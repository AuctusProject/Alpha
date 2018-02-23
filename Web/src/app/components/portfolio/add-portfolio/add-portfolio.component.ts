import { Component, OnInit } from '@angular/core';
import { PortfolioRequest } from '../../../model/portfolio/portfolioRequest';
import { LoginService } from '../../../services/login.service';
import { Asset } from '../../../model/asset/asset';
import { PublicService } from '../../../services/public.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-portfolio',
  templateUrl: './add-portfolio.component.html',
  styleUrls: ['./add-portfolio.component.css']
})
export class AddPortfolioComponent implements OnInit {
  portfolio: PortfolioRequest;
  loginData: any;
  assets: Asset[];

  constructor(private loginService: LoginService, private publicService: PublicService, private router: Router) { }

  ngOnInit() {
    this.publicService.listAssets().subscribe(assets => this.assets = assets);

    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.loginData = this.loginService.getLoginData();
      if (this.loginData) {
        this.portfolio = new PortfolioRequest();
        this.portfolio.advisorId = this.loginData.humanAdvisorId;
        this.portfolio.isEditing = true;
        return;
      }
    }


    this.loginService.logout();
  }

  public onPortfolioSaved() {
    this.router.navigateByUrl('advisor/' + this.loginData.humanAdvisorId);
  }
}
