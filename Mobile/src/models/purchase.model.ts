import { ProjectionData } from './projectionData.model';
import { Goal } from './goal.model';

export class Purchase {

    advisorId: number;
    description: string;
    expirationDate: Date;
    goal: Goal;
    name: string;
    period: number;
    price: number;
    purchaseDate: Date;
    projectionData: ProjectionData;

    public Purchase() {
        this.advisorId = 0;
        this.description = '';
        this.expirationDate = null;
        this.goal = new Goal();
        this.name = '';
        this.period = 0;
        this.price = 0;
        this.purchaseDate = null;
        this.projectionData = new ProjectionData();
    }
}