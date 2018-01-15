import { Goal } from "../goal";
import { Purchase } from "./purchase";

export class Projections {
  currentGoal: Goal;
  purchase: Purchase[];

  constructor(){
    this.purchase = [];
  }
}
