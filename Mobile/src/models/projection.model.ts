
import { Goal } from "./goal.model";
import { Purchase } from "./purchase.model";

export class Projection {

    currentGoal: Goal;
    purchases: Array<Purchase>;

    public Projection() {
        this.currentGoal = new Goal();
        this.purchases = new Array<Purchase>();
    }
}