import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";

@Component({
  selector: 'period-step',
  templateUrl: './period-step.component.html',
  styleUrls: ['./period-step.component.css']
})
export class PeriodStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
    this.model.timeframe = 0;
  }

  onSubmit(){
    this.onSubmitted.emit(true);
  }

  onChangeSlider(){
    console.log(this.model);
  }
}
