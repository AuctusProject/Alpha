import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'

@Component({
  selector: 'advisor-step2',
  templateUrl: './step2.component.html',
  styleUrls: ['./step2.component.css']
})
export class Step2Component implements OnInit {
  payFrequencies = [
    { value: '7', viewValue: 'Weekly' },
    { value: '30', viewValue: 'Monthly' }
  ];

  @Input() model: Advisor;
  @Output() onStepFinished = new EventEmitter<Advisor>();

  constructor() { }

  ngOnInit() {
  }


  public back() {
    this.onStepFinished.emit(null);
  }

  public next() {
    this.onStepFinished.emit(this.model);
  }
}
