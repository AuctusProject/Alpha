import { AdvisorWizardStep } from './advisor-wizard-step.enum';
import { Advisor } from './../../../model/advisor/advisor';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-advisor-wizard',
  templateUrl: './advisor-wizard.component.html',
  styleUrls: ['./advisor-wizard.component.css']
})

export class AdvisorWizardComponent implements OnInit {

  public currentStep;
  public advisorModel: Advisor;
  public editedAdvisorModel: Advisor;
  public wizardSteps = AdvisorWizardStep;

  constructor() {
  }

  ngOnInit() {
    this.advisorModel = new Advisor();
    this.currentStep = this.wizardSteps.Start.Id;
  }

  public changeStep(stepToChange) {
    this.currentStep = stepToChange;
  }

  public backStep() {
    switch (this.currentStep) {
      case this.wizardSteps.Advisor.Id:
        this.currentStep = this.wizardSteps.Start.Id;
        break;
      case this.wizardSteps.Portfolio.Id:
        this.currentStep = this.wizardSteps.Advisor.Id;
        break;
    }
  }

  public nextStep() {

    switch (this.currentStep) {
      case this.wizardSteps.Start.Id:
        this.currentStep = this.wizardSteps.Advisor.Id;
        this.editedAdvisorModel = JSON.parse(JSON.stringify(this.advisorModel));
        break;
      case this.wizardSteps.Advisor.Id:
        this.currentStep = this.wizardSteps.Portfolio.Id;
        this.advisorModel = JSON.parse(JSON.stringify(this.editedAdvisorModel));
        break;
      case this.wizardSteps.Portfolio.Id:
        this.currentStep = this.wizardSteps.Start.Id;
        break;
    }
  }
}
