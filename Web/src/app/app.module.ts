import { BrowserModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChartsModule } from 'ng2-charts';

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
import { MatModule } from './mat.module';

import { AppComponent } from './app.component';
import { AccountService } from './services/account.service';
import { StepperComponent } from './components/ui/stepper/stepper.component';
import { RegisterStepComponent } from './components/register/register-step/register-step.component';
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { PendingEmailConfirmationComponent } from './components/account/pending-email-confirmation/pending-email-confirmation.component';
import { AdvisorService } from './services/advisor.service';
import { PortfolioService } from './services/portfolio.service';
import { FocusDirective } from './directives/focus.directive';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { WizardHeaderComponent } from './components/register/wizard-header/wizard-header.component';
import { HomeComponent } from './components/home/home.component';
import { PaddingDirective } from './directives/padding.directive';
import { DashboardComponent } from './components/dashboard/dashboard/dashboard.component';
import { GoalHeaderComponent } from './components/dashboard/goal-header/goal-header.component';
import { ProjectionTabComponent } from './components/dashboard/projection-tab/projection-tab.component';
import { GoalOptionComponent } from './components/register/goal-step/goal-option/goal-option.component';
import { WindowRefService } from './services/window-ref.service';
import { ActionBarComponent } from './components/dashboard/action-bar/action-bar.component';
import { PortfolioTabComponent } from './components/dashboard/portfolio-tab/portfolio-tab.component';
import { HistoricalTabComponent } from './components/dashboard/historical-tab/historical-tab.component';

import 'hammerjs';
import { PortfolioHistoryChartComponent } from './components/dashboard/portfolio-history-chart/portfolio-history-chart.component';
import { PortfolioDistributionComponent } from './components/dashboard/portfolio-distribution/portfolio-distribution.component';
import { PortfolioHistogramComponent } from './components/dashboard/portfolio-histogram/portfolio-histogram.component';
import { ChangePasswordComponent } from './components/account/change-password/change-password.component';
import { ManageApiComponent } from './components/account/manage-api/manage-api.component';
import { AccountHeaderComponent } from './components/account/account-header/account-header.component';

import { IconsModule } from './icons.module';
import { AdvisorWizardComponent } from './components/account/advisor-wizard/advisor-wizard.component';
import { AdvisorStepComponent } from './components/account/advisor-wizard/advisor-step/advisor-step.component';
import { PortfolioStepComponent } from './components/account/advisor-wizard/portfolio-step/portfolio-step.component';
import { LoadingBlockComponent } from './components/ui/loading-block/loading-block.component';
import { EthMachineComponent } from './components/eth-machine/eth-machine.component';
import { PortfolioRegisterComponent } from './components/account/advisor-wizard/portfolio-register/portfolio-register.component';
import { PortfolioDistributionRowComponent } from './components/account/advisor-wizard/portfolio-register/portfolio-distribution-row/portfolio-distribution-row.component';
import { Web3Service } from './services/web3.service';
import { PublicService } from './services/public.service';
import { MetamaskAccountService } from './services/metamask-account.service';
import { CanActivateViaAuthGuard } from './providers/canActivateViaAuth.provider';
import { UserProfileAuthGuard } from './providers/user-profile-auth-guard.provider';
import { ProviderRequiredComponent } from './components/provider-required/provider-required.component';
import { EventsServiceModule } from 'angular-event-service';
import { MetamaskAccountMonitorComponent } from './components/metamask-account-monitor/metamask-account-monitor.component';
import { EmailRegistrationDirective } from './directives/email-registration.directive';
import { UsernameRegistrationDirective } from './directives/username-registration.directive';
import { RoboAdvisorsComponent } from './components/portfolio/robo-advisors/robo-advisors.component';
import { HumanAdvisorsComponent } from './components/portfolio/human-advisors/human-advisors.component';
import { PortfolioCardComponent } from './components/portfolio/portfolio-card/portfolio-card.component';
import { AdvisorPortfoliosComponent } from './components/advisor/advisor-portfolios/advisor-portfolios.component';
import { AdvisorDetailsComponent } from './components/advisor/advisor-details/advisor-details.component';

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
    PendingEmailConfirmationComponent,
    FocusDirective,
    ConfirmEmailComponent,
    WizardHeaderComponent,
    HomeComponent,
    PaddingDirective,
    DashboardComponent,
    GoalHeaderComponent,
    ProjectionTabComponent,
    GoalOptionComponent,
    ActionBarComponent,
    PortfolioTabComponent,
    HistoricalTabComponent,
    PortfolioHistoryChartComponent,
    PortfolioDistributionComponent,
    PortfolioHistogramComponent,
    ChangePasswordComponent,
    ManageApiComponent,
    AccountHeaderComponent,
    AdvisorWizardComponent,
    AdvisorStepComponent,
    PortfolioStepComponent,
    LoadingBlockComponent,
    EthMachineComponent,
    PortfolioRegisterComponent,
    PortfolioDistributionRowComponent,
    ProviderRequiredComponent,
    MetamaskAccountMonitorComponent,
    EmailRegistrationDirective,
    UsernameRegistrationDirective,
    RoboAdvisorsComponent,
    HumanAdvisorsComponent,
    PortfolioCardComponent,
    AdvisorPortfoliosComponent,
    AdvisorDetailsComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    MatModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    AppRoutingModule,
    FlexLayoutModule,
    ChartsModule,
    IconsModule,
    Angular2PromiseButtonModule
      .forRoot({
        spinnerTpl: '<span class="btn-spinner"></span>',
        disableBtn: true,
        btnLoadingClass: 'is-loading',
        handleCurrentBtnOnly: true,
    }),
    SimpleNotificationsModule.forRoot(),
    EventsServiceModule.forRoot()
  ],
  providers: [
    LoginService, 
    HttpService, 
    AccountService, 
    AdvisorService, 
    PortfolioService, 
    WindowRefService, 
    PublicService, 
    Web3Service, 
    CanActivateViaAuthGuard,
    UserProfileAuthGuard,
    MetamaskAccountService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
