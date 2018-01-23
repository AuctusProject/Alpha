import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";
import {MediaChange, ObservableMedia} from "@angular/flex-layout";

@Component({
  selector: 'period-step',
  templateUrl: './period-step.component.html',
  styleUrls: ['./period-step.component.css']
})
export class PeriodStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();

  constructor(public media: ObservableMedia) { }

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
