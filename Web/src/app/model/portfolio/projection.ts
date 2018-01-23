import { Result } from "./result";

export class Projection {
  risk: number;
  projectionPercent: number;
  optimisticPercent: number;
  pessimisticPercent: number;
  current: Result;
  purchased: Result;

  constructor(){
  }
}
