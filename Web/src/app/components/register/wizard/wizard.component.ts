import { Component, OnInit } from '@angular/core';
import { Goal } from "../../../model/goal";
import { GoalOption } from '../../../model/goalOption';

@Component({
  selector: 'register-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {
  isLinear = true;
  goal: Goal;

  constructor() {
    this.goal = new Goal(0, 0, 0, 0, new GoalOption(0, '', 0, 0));
  }

  ngOnInit() {
    
  }

  onClickButton(){
    console.log(this.goal);
  } 
}
