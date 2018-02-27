
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'
import { AdvisorService } from '../../../../services/advisor.service'
import { LoginService } from '../../../../services/login.service'
import { AdvisorWizardStep } from './../advisor-wizard-step.enum';


@Component({
  selector: 'advisor-step',
  templateUrl: './advisor-step.component.html',
  styleUrls: ['./advisor-step.component.css']
})
export class AdvisorStepComponent implements OnInit {


  public wizardSteps = AdvisorWizardStep;
  @Input() advisorModel: Advisor;
  @Output() onBackStep = new EventEmitter<Advisor>();
  @Output() onNextStep = new EventEmitter<Advisor>();

  constructor(private advisorService : AdvisorService, private loginService : LoginService) { }

  ngOnInit() {
  }

  public back() {
    this.onBackStep.emit();
  }

  public next() {
    this.onNextStep.emit();
  }

  onSubmit() {
    if (this.advisorModel.id) {
      this.advisorService.updateAdvisor(this.advisorModel).subscribe(val => this.next());
    }
    else {
      this.advisorService.createAdvisor(this.advisorModel).subscribe(
        advisor => this.afterSave(advisor));
    }
  }

  afterSave(advisor) {
    let loginData = this.loginService.getLoginData();
    loginData.humanAdvisorId = advisor.id;
    this.loginService.setLoginData(loginData);
    this.advisorModel.id = advisor.id;
    this.next();
  }

}
