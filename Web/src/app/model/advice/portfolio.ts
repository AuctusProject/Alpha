import { RiskType } from "../account/riskType";
import { Projection } from "./projection";

export class Portfolio {
  id: number;
  advisorId: number;
  risk: number;
  creationDate: Date;
  disabled: Date;
  riskType: RiskType;
  projection: Projection;

  constructor(){
    this.riskType = new RiskType();
    this.projection = new Projection();
  }
}
