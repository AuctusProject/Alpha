import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'goal-step',
  templateUrl: './goal-step.component.html',
  styleUrls: ['./goal-step.component.css']
})
export class GoalStepComponent implements OnInit {

  @Input() form: FormGroup;

  constructor() { }

  ngOnInit() {
  }

}
