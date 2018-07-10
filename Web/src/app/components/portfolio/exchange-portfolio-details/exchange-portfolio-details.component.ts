import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { PortfolioService } from "../../../services/portfolio.service";
import { Portfolio } from "../../../model/portfolio/portfolio";
import { ExchangePortfolio } from "../../../model/portfolio/exchangePortfolio";
import { NotificationsService } from "angular2-notifications";
import { Subscription } from "rxjs";

@Component({
  selector: "app-exchange-portfolio-details",
  templateUrl: "./exchange-portfolio-details.component.html",
  styleUrls: ["./exchange-portfolio-details.component.css"]
})
export class ExchangePortfolioDetailsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private portfolioService: PortfolioService,
    private notificationService: NotificationsService
  ) {}
  portfolio: ExchangePortfolio;
  deletePromise: Subscription;

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.getExchangePortfolio(params["id"]);
    });
  }

  getExchangePortfolio(exchangeId) {
    this.portfolioService
      .getExchangePortfolio(exchangeId)
      .subscribe(portfolio => (this.portfolio = portfolio));
  }

  deleteExchangePortfolio() {
    this.deletePromise = this.portfolioService
      .deleteExchangePortfolio(this.portfolio.exchangeId)
      .subscribe(result => {
        this.notificationService.success("Success", "Exchange portfolio deleted.");
        this.router.navigateByUrl('following');
      });
  }
}
