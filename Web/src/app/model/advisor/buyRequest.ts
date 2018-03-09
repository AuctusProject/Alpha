import { Goal } from "../account/goal";

export class BuyRequest {
    portfolioId: number;
    days : number;
    address : string;
    invested : number;
    goal : Goal;

    constructor(portfolioId: number, days: number, address: string, invested : number, goal?: Goal){
      this.portfolioId = portfolioId;
      this.days = days;
      this.address = address;
      this.invested = invested;
      this.goal = goal;
    }
  }
  