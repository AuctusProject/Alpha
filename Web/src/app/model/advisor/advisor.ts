import { AdvisorDetail } from "./advisorDetail";
import { Projection } from "../portfolio/projection";

export class Advisor {
  id: number;
  name: string;
  description: string;
  price: number;
  period: number;
  purchased: boolean;
  purchaseQuantity: number;
  projection: Projection[];
  userId: number;
  detail: AdvisorDetail;

  constructor() {
    this.projection = [];
  }
}
