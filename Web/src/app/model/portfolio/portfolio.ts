import { RiskType } from "../account/riskType";
import { Projection } from "./projection";
import { HistoryResult } from "../advisor/historyResult";
import { AssetDistribution } from "../asset/assetDistribution";
import { HistoryValues } from "../advisor/historyValues";
import { HistogramInfo } from "../advisor/histogramInfo";
import { BasePortfolio } from "./basePortfolio";

export class Portfolio extends BasePortfolio {
  id: number;
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
  advisorType: number;
  risk: number;
  projectionPercent: number;
  optimisticPercent: number;
  pessimisticPercent: number;
  totalDays: number;
  lastDay: HistoryResult;
  last7Days: HistoryResult;
  last30Days: HistoryResult;
  allDays: HistoryResult;

  constructor(){
    super();
    this.lastDay = new HistoryResult();
    this.last7Days = new HistoryResult();
    this.last30Days = new HistoryResult();
    this.allDays = new HistoryResult();
  }
}
