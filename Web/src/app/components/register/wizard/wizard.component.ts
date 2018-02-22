import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";
import { GoalOption } from '../../../model/account/goalOption';
import { MatStepper } from "@angular/material";

@Component({
  selector: 'register-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {
  @Output() onSubmitted = new EventEmitter<Goal>();
  isLinear = true;
  goal: Goal;

  constructor() {
    this.goal = new Goal();
  }

  ngOnInit() {

  }

  onClickButton() {
    console.log(this.goal);
  }

  goBack(stepper: MatStepper) {
    stepper.previous();
  }

  goForward(stepper: MatStepper) {
    stepper.next();
  }

  onLastStepSubmitted(goal: Goal) {
    this.onSubmitted.emit(goal);
  }

  onStepChange(){
  }

  goToStep(step : number, stepper : MatStepper){
    stepper.selectedIndex = step;
  }
}
