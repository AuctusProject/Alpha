import { Goal } from "../goal";
import { User } from "./user";

export class FullRegister {
  user : User;
  goal : Goal;

  constructor(){
    this.user = new User();
    this.goal = new Goal();
  }
}
