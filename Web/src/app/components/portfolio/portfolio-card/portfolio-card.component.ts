import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from "../../../model/portfolio/portfolio";

@Component({
  selector: 'portfolio-card',
  templateUrl: './portfolio-card.component.html',
  styleUrls: ['./portfolio-card.component.css']
})
export class PortfolioCardComponent implements OnInit {
  @Input() portfolio: Portfolio;
  constructor() { }

  ngOnInit() {
  }

  private getRiskDescription() {
    if (this.portfolio.risk == 1) {
      return "Conservative";
    }
    else if (this.portfolio.risk == 2) {
      return "Moderately Conservative";
    }
    else if (this.portfolio.risk == 3) {
      return "Moderately Aggressive";
    }
    else if (this.portfolio.risk == 4) {
      return "Aggressive";
    }
    else if (this.portfolio.risk == 5) {
      return "Very Aggressive"
    }
  }
}
