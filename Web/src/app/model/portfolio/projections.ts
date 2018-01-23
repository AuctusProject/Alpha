import { Goal } from "../account/goal";
import { Purchase } from "../advisor/purchase";

export class Projections {
  currentGoal: Goal;
  purchase: Purchase[];

  constructor(){
    this.purchase = [];
  }
}
