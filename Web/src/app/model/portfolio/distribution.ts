import { Asset } from "../asset/asset";

export class Distribution {
  id: number;
  asset: Asset;
  percentual: number;

  constructor(){
    this.asset = new Asset();
  }
}
