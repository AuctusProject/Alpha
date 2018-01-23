import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";

@Component({
  selector: 'amount-step',
  templateUrl: './amount-step.component.html',
  styleUrls: ['./amount-step.component.css']
})
export class AmountStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  onSubmit(){
    this.onSubmitted.emit(true);
  }
}
