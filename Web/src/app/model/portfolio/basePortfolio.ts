import { AssetDistribution } from "../asset/assetDistribution";
import { HistoryValues } from "../advisor/historyValues";
import { HistogramInfo } from "../advisor/histogramInfo";
import { AssetDistributionHistory } from "../asset/assetDistributionHistory";

export class BasePortfolio {
  name: string;
  assetDistribution: AssetDistribution[];
  assetDistributionHistory: AssetDistributionHistory[];
  historyData: HistoryValues[];
  histogram: HistogramInfo[];

  constructor(){
    this.assetDistribution = [];
    this.historyData = [];
    this.histogram = [];
  }
}
