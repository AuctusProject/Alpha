import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './/app-routing.module';
import { LoginComponent } from './components/login/login.component';
import { LoginService } from './services/login.service';
import { HttpService } from './services/http.service';
import { HttpClientModule } from '@angular/common/http';
import { WizardComponent } from './components/register/wizard/wizard.component';
import { GoalStepComponent } from './components/register/goal-step/goal-step.component';
import { PeriodStepComponent } from './components/register/period-step/period-step.component';
import { AmountStepComponent } from './components/register/amount-step/amount-step.component';
import { RiskStepComponent } from './components/register/risk-step/risk-step.component';
import { MatModule } from './/mat.module';

import { AppComponent } from './app.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    GoalStepComponent,
    PeriodStepComponent,
    AmountStepComponent,
    RiskStepComponent,
    WizardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    AppRoutingModule,
    FlexLayoutModule
  ],
  providers: [LoginService, HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
