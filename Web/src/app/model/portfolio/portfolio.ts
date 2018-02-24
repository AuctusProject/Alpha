import { RiskType } from "../account/riskType";
import { Projection } from "./projection";
import { HistoryResult } from "../advisor/historyResult";
import { AssetDistribution } from "../asset/assetDistribution";

export class Portfolio {
  id: number;
  name: string;
  description: string;
  price: number;
  purchased: boolean;
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

  constructor(){
    this.lastDay = new HistoryResult();
    this.last7Days = new HistoryResult();
    this.last30Days = new HistoryResult();
    this.allDays = new HistoryResult();
    this.assetDistribution = [];
  }
}
