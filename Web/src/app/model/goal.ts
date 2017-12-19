import { GoalOption } from "./goalOption";

export class Goal {

  constructor(
    public id: number,
    public targetAmount: number,
    public startingAmount: number,
    public monthlyContribution: number,
    public timeFrame: number,
    public risk: number,
    public GoalOption: GoalOption
  ) { }
}
