export class Goal {

    monthlyContribution: number;
    risk: number;
    startingAmount: number;
    targetAmount: number;
    timeframe: number;

    public Goal() {
        this.monthlyContribution = 0;
        this.risk = 0;
        this.startingAmount = 0;
        this.targetAmount = 0;
        this.timeframe = 0;
    }
}