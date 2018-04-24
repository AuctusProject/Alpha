import { Component, OnInit, Input } from '@angular/core';
import { ExchangePortfolio } from '../../../model/portfolio/exchangePortfolio';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../../services/local-storage.service';

@Component({
  selector: 'exchange-portfolio-card',
  templateUrl: './exchange-portfolio-card.component.html',
  styleUrls: ['./exchange-portfolio-card.component.css']
})
export class ExchangePortfolioCardComponent implements OnInit {
  @Input() exchangePortfolio: ExchangePortfolio;

  constructor(private router: Router, private localStorageService: LocalStorageService) { }

  ngOnInit() {
  }

  onDetailsClick() {
    this.router.navigateByUrl("/exchange-portfolio/" + this.exchangePortfolio.exchangeId);
  }
}
