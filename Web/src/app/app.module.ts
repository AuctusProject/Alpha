import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SimpleNotificationsModule } from 'angular2-notifications';
import { Angular2PromiseButtonModule } from 'angular2-promise-buttons/dist';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './/app-routing.module';
import { LoginComponent } from './components/account/login/login.component';
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
import { AccountService } from './services/account.service';
import { StepperComponent } from './components/ui/stepper/stepper.component';
import { RegisterStepComponent } from './components/register/register-step/register-step.component';
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { AdvisorDetailComponent } from './components/advice/advisor-detail/advisor-detail.component';
import { AdvisorsComponent } from './components/advice/advisors/advisors.component';
import { AdviceService } from './services/advice.service';
import { FocusDirective } from './directives/focus.directive';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { WizardHeaderComponent } from './components/register/wizard-header/wizard-header.component';
import { HomeComponent } from './components/home/home.component';
import { PaddingDirective } from './directives/padding.directive';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    GoalStepComponent,
    PeriodStepComponent,
    AmountStepComponent,
    RiskStepComponent,
    WizardComponent,
    StepperComponent,
    RegisterStepComponent,
    ForgotPasswordEmailComponent,
    ForgotPasswordResetComponent,
    AdvisorDetailComponent,
    AdvisorsComponent,
    FocusDirective,
    ConfirmEmailComponent,
    WizardHeaderComponent,
    HomeComponent,
    PaddingDirective
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
    FlexLayoutModule,
    Angular2PromiseButtonModule
      .forRoot({
        spinnerTpl: '<span class="btn-spinner"></span>',
        disableBtn: true,
        btnLoadingClass: 'is-loading',
        handleCurrentBtnOnly: true,
    }),
    SimpleNotificationsModule.forRoot()
  ],
  providers: [LoginService, HttpService, AccountService, AdviceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
