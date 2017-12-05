import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './/app-routing.module';
import { LoginComponent } from './components/login/login.component';
import { LoginService } from './services/login.service';
import { HttpService } from './services/http.service';
import { HttpClientModule } from '@angular/common/http';
import { WizardComponent } from './components/register/wizard/wizard.component';
import { StepsComponent } from './components/register/steps/steps.component';
import { GoalStepComponent } from './components/register/goal-step/goal-step.component';
import { PeriodStepComponent } from './components/register/period-step/period-step.component';
import { AmountStepComponent } from './components/register/amount-step/amount-step.component';
import { RiskStepComponent } from './components/register/risk-step/risk-step.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    StepsComponent,
    GoalStepComponent,
    PeriodStepComponent,
    AmountStepComponent,
    RiskStepComponent,
    WizardComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [LoginService, HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
