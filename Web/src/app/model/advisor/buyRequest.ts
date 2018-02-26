import { Goal } from "../account/goal";

export class BuyRequest {
    portfolioId: number;
    days : number;
    address : string;
    goal : Goal;

    constructor(portfolioId: number, days: number, address: string, goal?: Goal){
      this.portfolioId = portfolioId;
      this.days = days;
      this.address = address;
      this.goal = goal;
    }
  }
  