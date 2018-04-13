import { Portfolio } from "./portfolio";
import { ExchangePortfolio } from "./exchangePortfolio";

export class Investments {
  purchasedPortfolios: Portfolio[];
  exchangePortfolios: ExchangePortfolio[];

  constructor(){
    this.purchasedPortfolios = [];
    this.exchangePortfolios = [];
  }
}
