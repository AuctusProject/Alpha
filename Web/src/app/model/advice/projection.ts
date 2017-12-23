import { Distribution } from "./distribution";

export class Projection {
  id: number;
  date: Date;
  projectionValue: number;
  optimisticProjection: number;
  pessimisticProjection: number;
  distribution: Distribution[];

  constructor(){
    this.distribution = [];
  }
}
