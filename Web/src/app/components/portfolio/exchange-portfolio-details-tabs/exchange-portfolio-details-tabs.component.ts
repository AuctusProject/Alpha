import { Component, OnInit, Input } from '@angular/core';
import { ExchangePortfolio } from '../../../model/portfolio/exchangePortfolio';

@Component({
  selector: 'exchange-portfolio-details-tabs',
  templateUrl: './exchange-portfolio-details-tabs.component.html',
  styleUrls: ['./exchange-portfolio-details-tabs.component.css']
})
export class ExchangePortfolioDetailsTabsComponent implements OnInit {

  @Input() portfolio: ExchangePortfolio;
  selectedIndex : number;
  
  constructor() { }

  ngOnInit() {
    this.selectedIndex = 0;
  }

}
