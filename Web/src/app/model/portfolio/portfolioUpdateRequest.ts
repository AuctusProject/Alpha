import { DistributionRequest } from "./distributionRequest";

export class PortfolioUpdateRequest {
  name: string;
  description: string;  
  price: number;
  distribution: DistributionRequest[];

  constructor() {
    this.distribution = [];
  }
}
