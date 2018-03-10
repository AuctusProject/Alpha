import { BrowserModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChartsModule } from 'ng2-charts';

import { SimpleNotificationsModule } from 'angular2-notifications';
import { Angular2PromiseButtonModule } from 'angular2-promise-buttons/dist';
import { NgCircleProgressModule } from 'ng-circle-progress';

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
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { PendingEmailConfirmationComponent } from './components/account/pending-email-confirmation/pending-email-confirmation.component';
import { AdvisorService } from './services/advisor.service';
import { PortfolioService } from './services/portfolio.service';
import { FocusDirective } from './directives/focus.directive';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { HomeComponent } from './components/home/home.component';
import { PaddingDirective } from './directives/padding.directive';
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

import { IconsModule } from './icons.module';
import { AdvisorWizardComponent } from './components/account/advisor-wizard/advisor-wizard.component';
import { AdvisorStepComponent } from './components/account/advisor-wizard/advisor-step/advisor-step.component';
import { PortfolioStepComponent } from './components/account/advisor-wizard/portfolio-step/portfolio-step.component';
import { LoadingBlockComponent } from './components/ui/loading-block/loading-block.component';
import { EthMachineComponent } from './components/eth-machine/eth-machine.component';
import { PortfolioRegisterComponent } from './components/account/advisor-wizard/portfolio-register/portfolio-register.component';
import { PortfolioDistributionRowComponent } from './components/account/advisor-wizard/portfolio-register/portfolio-distribution-row/portfolio-distribution-row.component';
import { Web3Service } from './services/web3.service';
import { LocalStorageService } from './services/local-storage.service';
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
import { PortfolioDetailsComponent } from './components/portfolio/portfolio-details/portfolio-details.component';
import { RiskIndicatorComponent } from './components/portfolio/risk-indicator/risk-indicator.component';
import { HeaderComponent } from './components/ui/header/header.component';
import { MyInvestmentsComponent } from './components/portfolio/my-investments/my-investments.component';
import { FooterComponent } from './components/ui/footer/footer.component';
import { AddPortfolioComponent } from './components/portfolio/add-portfolio/add-portfolio.component';
import { PortfolioReturnIndicatorComponent } from './components/portfolio/portfolio-return-indicator/portfolio-return-indicator.component';
import { PortfolioDetailsTabsComponent } from './components/portfolio/portfolio-details-tabs/portfolio-details-tabs.component';
import { PortfolioProjectionComponent } from './components/portfolio/portfolio-projection/portfolio-projection.component';
import { PortfolioHistoryComponent } from './components/portfolio/portfolio-history/portfolio-history.component';
import { PortfolioAllocationComponent } from './components/portfolio/portfolio-allocation/portfolio-allocation.component';
import { PortfolioPurchaseComponent } from './components/portfolio/portfolio-purchase/portfolio-purchase.component';
import { PortfolioPurchasePopupComponent } from './components/portfolio/portfolio-purchase-popup/portfolio-purchase-popup.component';
import { PercentageLabelComponent } from './components/ui/percentage-label/percentage-label.component';
import { PortfolioProjectionChartComponent } from './components/dashboard/portfolio-projection-chart/portfolio-projection-chart.component';
import { LoadingSpinnerComponent } from './components/ui/loading-spinner/loading-spinner.component';
import { MobileNotSupportedComponent } from './components/ui/mobile-not-supported/mobile-not-supported.component';
import { SetHashPopupComponent } from './components/portfolio/set-hash-popup/set-hash-popup.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { AdvisorPerformanceComponent } from './components/ranking/advisor-performance/advisor-performance.component';
import { UserBalanceComponent } from './components/user-balance/user-balance.component';
import { TelegramValidatorComponent } from './components/home/telegram-validator/telegram-validator.component';
import { UserPerformanceComponent } from './components/ranking/user-performance/user-performance.component';
import { UserCardComponent } from './components/ranking/user-performance/user-card/user-card.component';
import { FilterPipe } from './pipes/filter.pipe';

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
    ForgotPasswordEmailComponent,
    ForgotPasswordResetComponent,
    PendingEmailConfirmationComponent,
    FocusDirective,
    ConfirmEmailComponent,
    HomeComponent,
    PaddingDirective,
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
    AdvisorDetailsComponent,
    PortfolioDetailsComponent,
    RiskIndicatorComponent,
    HeaderComponent,
    MyInvestmentsComponent,
    FooterComponent,
    AddPortfolioComponent,
    PortfolioReturnIndicatorComponent,
    PortfolioDetailsTabsComponent,
    PortfolioProjectionComponent,
    PortfolioHistoryComponent,
    PortfolioAllocationComponent,
    PortfolioPurchaseComponent,
    PortfolioPurchasePopupComponent,
    PercentageLabelComponent,
    PortfolioProjectionChartComponent,
    LoadingSpinnerComponent,
    MobileNotSupportedComponent,
    SetHashPopupComponent,
    AdvisorPerformanceComponent,
    UserBalanceComponent,
    TelegramValidatorComponent,
    UserPerformanceComponent,
    UserCardComponent,
    FilterPipe
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
    CurrencyMaskModule,
    Angular2PromiseButtonModule
      .forRoot({
        spinnerTpl: '<span class="btn-spinner"></span>',
        disableBtn: true,
        btnLoadingClass: 'is-loading',
        handleCurrentBtnOnly: true,
    }),
    SimpleNotificationsModule.forRoot(),
    EventsServiceModule.forRoot(),
    NgCircleProgressModule.forRoot()
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
    MetamaskAccountService,
    LocalStorageService
  ],
  entryComponents: [PortfolioPurchasePopupComponent, SetHashPopupComponent, TelegramValidatorComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
