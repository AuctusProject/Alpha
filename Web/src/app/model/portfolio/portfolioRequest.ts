import { DistributionRequest } from "./distributionRequest";

export class PortfolioRequest {
  advisorId: number;
  risk: number;
  projectionValue: number;
  optimisticProjection: number;
  pessimisticProjection: number;
  distribution: DistributionRequest[];

  constructor() {
    this.distribution = [];
  }
}
