import { Component, OnInit } from '@angular/core';
import { Advisor } from '../../../model/advisor/advisor'

@Component({
  selector: 'app-advisor-wizard',
  templateUrl: './advisor-wizard.component.html',
  styleUrls: ['./advisor-wizard.component.css']
})
export class AdvisorWizardComponent implements OnInit {
  model: Advisor;
  step1Model: Advisor;
  step2Model: Advisor;
  step1EditingModel: Advisor;
  step2EditingModel: Advisor;

  public currentStep = 1;
  public editingStep = -1;
  constructor() { }

  ngOnInit() {
    this.model = new Advisor();
    this.step1Model = new Advisor();
    this.step2Model = new Advisor();
  }

  public nextStep() {
    this.fillEditingModel(this.currentStep);
    this.editingStep = this.currentStep;
  }

  public saveCurrentStep(stepModel) {
    if (this.currentStep == this.editingStep)
      this.currentStep++;

    if (this.editingStep == 1) {
      this.step1Model = stepModel;
    }
    if (this.editingStep == 2) {
      this.step2Model = stepModel;
    }
  }

  public fillEditingModel(stepIndex) {
    if (stepIndex == 1) {
      this.step1EditingModel = this.copyObject(this.step1Model);
    }
    if (stepIndex == 2) {
      this.step2EditingModel = this.copyObject(this.step2Model);
    }
  }

  public stepFinished(filledInformation, index) {
    if(filledInformation)
      this.saveCurrentStep(filledInformation);

    this.editingStep = -1;
  }

  public edit(stepIndex) {
    this.fillEditingModel(stepIndex);
    this.editingStep = stepIndex;
  }

  public textFromPeriod(model) {
    if (model.period == 7)
      return "week";
    if (model.period == 30)
      return "month";
    return model.period;
  }

  public copyObject(object) {
    return JSON.parse(JSON.stringify(object));
  }
}
