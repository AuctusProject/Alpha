import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'register-steps',
  templateUrl: './steps.component.html',
  styleUrls: ['./steps.component.css']
})
export class StepsComponent implements OnInit {

  @Input('currentStep') currentStep: number;

  constructor() { }

  ngOnInit() {
  }

}
