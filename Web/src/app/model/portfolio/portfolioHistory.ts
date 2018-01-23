import { HistoryValues } from "../advisor/historyValues";

export class PortfolioHistory {
  advisorId: number;
  values: HistoryValues[];

  constructor(){
    this.values = [];
  }
}
