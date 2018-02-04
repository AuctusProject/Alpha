import { AssetDistribution } from "../asset/assetDistribution";

export class PortfolioRequest {
  advisorId: number;
  risk: number;
  projectionValue: number;
  optimisticProjection: number;
  pessimisticProjection: number;
  distributionRequest: AssetDistribution[];

  constructor() {
    this.distributionRequest = [];
  }
}
