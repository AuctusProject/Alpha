import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'

@Component({
  selector: 'advisor-step1',
  templateUrl: './step1.component.html',
  styleUrls: ['./step1.component.css']
})
export class Step1Component implements OnInit {
  @Input() model: Advisor;
  isLinear = true;
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
