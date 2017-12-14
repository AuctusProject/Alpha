import { Component, OnInit, Input } from '@angular/core';
import { Goal } from "../../../model/goal";

@Component({
  selector: 'goal-step',
  templateUrl: './goal-step.component.html',
  styleUrls: ['./goal-step.component.css']
})
export class GoalStepComponent implements OnInit {

  @Input() model: Goal;

  constructor() { }

  ngOnInit() {
  }

}
