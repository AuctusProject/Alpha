import { AssetDistribution } from "../asset/assetDistribution";
import { HistoryValues } from "../advisor/historyValues";
import { HistogramInfo } from "../advisor/histogramInfo";

export class BasePortfolio {
  name: string;
  assetDistribution: AssetDistribution[];
  historyData: HistoryValues[];
  histogram: HistogramInfo[];

  constructor(){
    this.assetDistribution = [];
    this.historyData = [];
    this.histogram = [];
  }
}
