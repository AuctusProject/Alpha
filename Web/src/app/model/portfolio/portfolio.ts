import { RiskType } from "../account/riskType";
import { Projection } from "./projection";
import { HistoryResult } from "../advisor/historyResult";
import { AssetDistribution } from "../asset/assetDistribution";
import { HistoryValues } from "../advisor/historyValues";
import { HistogramInfo } from "../advisor/histogramInfo";

export class Portfolio {
  id: number;
  name: string;
  description: string;
  price: number;
  purchased: boolean;
  buyTransactionStatus: number;
  buyTransactionHash: string;
  buyTransactionId: number;
  owned: boolean;
  enabled: boolean;
  purchaseQuantity: number;
  advisorId: number;
  advisorName: string;
  advisorDescription: string;
  risk: number;
  projectionPercent: number;
  optimisticPercent: number;
  pessimisticPercent: number;
  totalDays: number;
  lastDay: HistoryResult;
  last7Days: HistoryResult;
  last30Days: HistoryResult;
  allDays: HistoryResult;
  assetDistribution: AssetDistribution[];
  historyData: HistoryValues[];
  histogram: HistogramInfo[];

  constructor(){
    this.lastDay = new HistoryResult();
    this.last7Days = new HistoryResult();
    this.last30Days = new HistoryResult();
    this.allDays = new HistoryResult();
    this.assetDistribution = [];
    this.historyData = [];
    this.histogram = [];
  }
}
