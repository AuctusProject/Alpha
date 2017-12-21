import { GoalOption } from "./goalOption";

export class Goal {

  id: number;
  targetAmount: number;
  startingAmount: number;
  monthlyContribution: number;
  timeFrame: number;
  risk: number;
  goalOption: GoalOption;
  
  constructor() {
    this.goalOption = new GoalOption();
  }
}
