import { HistoryValues } from "../advisor/historyValues";
import { AdvisorHistory } from "../advisor/advisorHistory";

export class PortfolioHistory {
  advisorId: number;
  history: AdvisorHistory;
  values: HistoryValues[];

  constructor(){
    this.values = [];
  }
}
