import { AssetDistribution } from "../asset/assetDistribution";

export class PortfolioDistribution {
  advisorId: number;
  distribution: AssetDistribution[];

  constructor(){
    this.distribution = [];
  }
}
