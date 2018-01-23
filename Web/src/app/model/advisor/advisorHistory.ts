import { HistoryResult } from "./historyResult";
import { HistogramInfo } from "./histogramInfo";

export class AdvisorHistory {
  risk: number;
  totalDays:number;
  lastDay: HistoryResult;
  last7Days: HistoryResult;
  last30Days: HistoryResult;
  allDays: HistoryResult;
  histogram: HistogramInfo[];

  constructor(){
    this.histogram = [];
  }  
}
