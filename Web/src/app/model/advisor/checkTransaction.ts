import { AssetDistribution } from "../asset/assetDistribution";

export class CheckTransaction {
  status: number;
  distribution: AssetDistribution[];

  constructor(){
    this.distribution = [];
  }
}
