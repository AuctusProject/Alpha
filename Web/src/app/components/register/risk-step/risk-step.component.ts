import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";
import {MediaChange, ObservableMedia} from "@angular/flex-layout";

@Component({
  selector: 'risk-step',
  templateUrl: './risk-step.component.html',
  styleUrls: ['./risk-step.component.css']
})
export class RiskStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();
  riskDescription: string;

  constructor(public media: ObservableMedia) { }

  ngOnInit() {
  }

  onChangeSlider(){
    if (this.model.risk == 1){
      this.riskDescription = "1 Avoiding loss is the priority";
    }
    else if (this.model.risk == 2){

    }
    else if (this.model.risk == 3){
      
    }
    else if (this.model.risk == 4){

    }
    else if (this.model.risk == 5){
      this.riskDescription = "5 High risk"
    }
  }

  onSubmit(){
    this.onSubmitted.emit(true);
  }

}
