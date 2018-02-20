import { Portfolio } from "../portfolio/portfolio";

export class Advisor {
  id: number;
  name: string;
  description: string;
  owned: boolean;
  enabled: boolean;
  purchaseQuantity: number;
  portfolios: Portfolio[];

  constructor() {
    this.portfolios = [];
  }
}
