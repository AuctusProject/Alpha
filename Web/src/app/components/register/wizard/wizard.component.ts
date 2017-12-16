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
    this.goal = new Goal(10, 10, 10, 10, 
      new GoalOption(4, "Child", 0, 1));
  }

  ngOnInit() {
    
  }
}
