import { Projection } from "./projection";
import { Goal } from "../goal";

export class Purchase {
  advisorId: number;
  purchaseDate: Date;
  expirationDate: Date;
  period: number;
  name: string;
  description: string;
  goal: Goal;
  projectionData: Projection[];

  constructor(){
    this.projectionData = [];
  }
}
