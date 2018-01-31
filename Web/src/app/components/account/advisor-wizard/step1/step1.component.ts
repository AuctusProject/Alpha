import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'
import { AdvisorService } from '../../../../services/advisor.service'


@Component({
  selector: 'advisor-step1',
  templateUrl: './step1.component.html',
  styleUrls: ['./step1.component.css']
})
export class Step1Component implements OnInit {
  payFrequencies = [
    { value: '7', viewValue: 'Weekly' },
    { value: '30', viewValue: 'Monthly' }
  ];

  @Input() model: Advisor;
  isLinear = true;
  @Output() onStepFinished = new EventEmitter<Advisor>();
  constructor(private advisorService : AdvisorService) { }

  ngOnInit() {
  }

  public back() {
    this.onStepFinished.emit(null);
  }

  public next() {
    this.onStepFinished.emit(this.model);
  }

  onSubmit() {
    if (this.model.id) {
      this.advisorService.updateAdvisor(this.model).subscribe(val => this.next());
    }
    else {
      this.advisorService.createAdvisor(this.model).subscribe(
        advisor => this.afterSave(advisor));
    }
  }

  afterSave(advisor) {
    this.model.id = advisor.id;
    this.next();
  }

}
