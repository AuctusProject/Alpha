import { Result } from "./result";
import { PortfolioDistribution } from "./portfolioDistribution";

export class Projection {
  risk: number;
  projectionPercent: number;
  optimisticPercent: number;
  pessimisticPercent: number;
  portfolioDistribution: PortfolioDistribution;
  current: Result;
  purchased: Result;

  constructor(){
  }
}
