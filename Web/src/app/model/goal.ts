import { GoalOption } from "./goalOption";

export class Goal {

  constructor(
    public Id: number,
    public TargetAmount: number,
    public StartingAmount: number,
    public MonthlyContribution: number,
    public GoalOption: GoalOption
  ) { }
}
