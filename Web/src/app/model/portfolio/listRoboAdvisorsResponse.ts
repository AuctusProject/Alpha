import { Portfolio } from "./portfolio"

export class ListRoboAdvisorsResponse {
  userRisk: number;
  portfolios: Portfolio[];

  constructor() {
    this.portfolios = [];
  }
}
