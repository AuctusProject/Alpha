import { HistoryValues } from "./historyValues";

export class PortfolioHistory {
  advisorId: number;
  values: HistoryValues[];

  constructor(){
    this.values = [];
  }
}
