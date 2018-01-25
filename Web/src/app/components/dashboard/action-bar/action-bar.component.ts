import { Component, OnInit, Input } from '@angular/core';
import { Projections } from "../../../model/portfolio/projections";
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";
import { PortfolioHistory } from "../../../model/portfolio/portfolioHistory";
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.css']
})
export class ActionBarComponent implements OnInit {
  @Input() projectionsModel: Projections;
  @Input() portfolioDistributionModel: PortfolioDistribution;
  @Input() portfolioHistoryModel: PortfolioHistory;
  @Input() user: string = null;

  constructor(private loginService: LoginService) { }

  ngOnInit() {
    this.user = this.loginService.getUser();
  }

  logout(): void {
    this.loginService.logout();
  }
}
