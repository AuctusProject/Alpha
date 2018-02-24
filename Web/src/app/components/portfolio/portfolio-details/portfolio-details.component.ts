import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { PortfolioService } from '../../../services/portfolio.service';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-portfolio-details',
  templateUrl: './portfolio-details.component.html',
  styleUrls: ['./portfolio-details.component.css']
})
export class PortfolioDetailsComponent implements OnInit {

  public portfolio: Portfolio;

  constructor(private portfolioService: PortfolioService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.getPortfolio(params['id']);
    });
  }

  private getPortfolio(portfolioId){
    this.portfolioService.getPortfolio(portfolioId)
    .subscribe(response => {
      console.log(response)
      this.portfolio = response
    })
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
