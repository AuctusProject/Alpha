import { Component, OnInit } from '@angular/core';
import { Advisor } from '../../../model/advisor/advisor'

@Component({
  selector: 'app-advisor-wizard',
  templateUrl: './advisor-wizard.component.html',
  styleUrls: ['./advisor-wizard.component.css']
})
export class AdvisorWizardComponent implements OnInit {
  model: Advisor;
  public currentStep = 1;
  public editingStep = -1;
  constructor() { }

  ngOnInit() {
    this.model = new Advisor();
  }

  public nextStep() {
    this.editingStep = this.currentStep;
  }

  public saveCurrentStep() {
    if (this.currentStep == this.editingStep)
      this.currentStep++;
  }

  public stepFinished(filledInformation, index) {
    if(filledInformation)
      this.saveCurrentStep();

    this.editingStep = -1;
  }

  public edit(stepIndex) {
    this.editingStep = stepIndex;
  }

  public textFromPeriod() {
    if (this.model.period == 7)
      return "week";
    if (this.model.period == 30)
      return "month";
    return this.model.period
  }
}
