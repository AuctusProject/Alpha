import { AdvisorWizardStep } from './advisor-wizard-step.enum';
import { Advisor } from './../../../model/advisor/advisor';
import { PortfolioRequest } from './../../../model/portfolio/portfolioRequest';
import { LoginService } from './../../../services/login.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RequestToBeAdvisor } from '../../../model/advisor/requestToBeAdvisor';
import { AdvisorService } from '../../../services/advisor.service';
import { Subscription } from 'rxjs/Subscription';
import { NotificationsService } from 'angular2-notifications';


@Component({
  selector: 'app-advisor-wizard',
  templateUrl: './advisor-wizard.component.html',
  styleUrls: ['./advisor-wizard.component.css']
})

export class AdvisorWizardComponent implements OnInit {

  public currentStep;
  public advisorModel: Advisor;
  public editedAdvisorModel: Advisor;
  public portfolioList: Array<PortfolioRequest>;
  public wizardSteps = AdvisorWizardStep;

  public requestModel: RequestToBeAdvisor;
  public submitPromise: Subscription;
  public submitBtnDescription: string;

  constructor(private router: Router, 
    private notificationService: NotificationsService,
    private advisorService: AdvisorService,
    private loginService: LoginService) {
    this.portfolioList = new Array<PortfolioRequest>();
    this.advisorModel = new Advisor();
    this.editedAdvisorModel = new Advisor();
    this.requestModel = new RequestToBeAdvisor();
  }

  ngOnInit() {
    this.currentStep = this.wizardSteps.Start.Id;

    this.advisorService.getRequestToBeAdvisor()
      .subscribe(response => {
        if (!!response) {
          this.submitBtnDescription = "UPDATE";
          this.requestModel = response;
        } else {
          this.submitBtnDescription = "SUBMIT";
        }
      });
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
        this.router.navigate(['advisor/' + this.loginService.getLoginData().humanAdvisorId]);
        break;
    }
  }

 public hasSavedPortfolio() {
   return this.getSavedPortfolioCount() > 0;
 }

  public getSavedPortfolioCount() {
     return this.portfolioList.filter(item => item.id > 0 ).length;
  }

  public getPortfoliosName() {
    let names = this.portfolioList.filter(item => item.id > 0 ).map(item => item.name);
    return names.join(", ");
  }

  public showMyPortoliosButton() {
    return this.currentStep === this.wizardSteps.Start.Id && this.hasSavedPortfolio();
  }

  public onMyPortfoliosClick() {
    this.router.navigateByUrl('advisor/'+this.loginService.getLoginData().humanAdvisorId);
  }

  public isApprovedRequest() {
    return !!this.requestModel.approved;
  }

  public submitRequest() {
    this.submitPromise = this.advisorService.requestToBeAdvisor(this.requestModel)
      .subscribe(response => {
        this.submitBtnDescription = "UPDATE";
        this.notificationService.success("Success", "Request submitted with success");
      });
  }
}
