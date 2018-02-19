import { DistributionRequest } from "./distributionRequest";

export class PortfolioRequest {
  id: number;
  advisorId: number;
  name: string;
  description: string;  
  risk: number;
  price: number;
  projectionValue: number;
  optimisticProjection: number;
  pessimisticProjection: number;
  distribution: DistributionRequest[];
  isEditing: boolean;

  constructor() {
    this.distribution = [];
  }
}
