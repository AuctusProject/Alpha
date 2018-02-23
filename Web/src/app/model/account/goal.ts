import { GoalOption } from "./goalOption";

export class Goal {

  id: number;
  targetAmount: number;
  startingAmount: number;
  monthlyContribution: number;
  timeframe: number;
  risk: number;
  goalOption: GoalOption;
  
  constructor() {
    this.startingAmount = 10000;
    this.monthlyContribution = 300;
    this.timeframe = 30;
    this.risk = 3;
    this.goalOption = new GoalOption();
  }
}
