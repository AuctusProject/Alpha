import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";

@Component({
  selector: 'risk-step',
  templateUrl: './risk-step.component.html',
  styleUrls: ['./risk-step.component.css']
})
export class RiskStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();

  constructor() { }

  ngOnInit() {
  }

  onChangeSlider(){
    console.log(this.model);
  }

}
