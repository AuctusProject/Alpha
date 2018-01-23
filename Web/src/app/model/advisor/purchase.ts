import { Projection } from "../portfolio/projection";
import { Goal } from "../account/goal";

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
