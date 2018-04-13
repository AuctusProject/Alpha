import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/account/login/login.component';
import { WizardComponent } from './components/register/wizard/wizard.component';
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { AdvisorDetailsComponent } from './components/advisor/advisor-details/advisor-details.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { HomeComponent } from "./components/home/home.component";
import { DashboardComponent } from "./components/dashboard/dashboard/dashboard.component";
import { ProjectionTabComponent } from "./components/dashboard/projection-tab/projection-tab.component";
import { PortfolioTabComponent } from "./components/dashboard/portfolio-tab/portfolio-tab.component";
import { HistoricalTabComponent } from "./components/dashboard/historical-tab/historical-tab.component";
import { ChangePasswordComponent } from './components/account/change-password/change-password.component';
import { ManageApiComponent } from './components/account/manage-api/manage-api.component';
import { AdvisorWizardComponent } from './components/account/advisor-wizard/advisor-wizard.component';
import { CanActivateViaAuthGuard } from './providers/canActivateViaAuth.provider';
import { ProviderRequiredComponent } from "./components/provider-required/provider-required.component";
import { RoboAdvisorsComponent } from "./components/portfolio/robo-advisors/robo-advisors.component";
import { HumanAdvisorsComponent } from "./components/portfolio/human-advisors/human-advisors.component";
import { UserProfileAuthGuard } from './providers/user-profile-auth-guard.provider';
import { PortfolioDetailsComponent } from "./components/portfolio/portfolio-details/portfolio-details.component";
import { MyInvestmentsComponent } from "./components/portfolio/my-investments/my-investments.component";
import { AddPortfolioComponent } from "./components/portfolio/add-portfolio/add-portfolio.component";
import { SyncExchangeComponent } from './components/portfolio/sync-exchange/sync-exchange.component';
import { ExchangePortfolioDetailsComponent } from './components/portfolio/exchange-portfolio-details/exchange-portfolio-details.component';

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent, canActivate: [UserProfileAuthGuard] },
    { path: 'required', component: ProviderRequiredComponent },
    { path: 'login', component: LoginComponent, canActivate: [UserProfileAuthGuard] },
    { path: 'forgot-password-email', component: ForgotPasswordEmailComponent },
    { path: 'forgot-password-reset', component: ForgotPasswordResetComponent },
    { path: 'confirm', component: ConfirmEmailComponent },
    { path: 'advisor/:id', component: AdvisorDetailsComponent, canActivate: [CanActivateViaAuthGuard] },
    { path: 'change-password', component: ChangePasswordComponent },
    { path: 'manage-api', component: ManageApiComponent, canActivate: [CanActivateViaAuthGuard] },
    { path: 'become-advisor', component: AdvisorWizardComponent },
    { path: 'robo-advisors', component: RoboAdvisorsComponent },
    { path: 'human-advisors', component: HumanAdvisorsComponent },
    { path: 'portfolio/:id', component: PortfolioDetailsComponent },
    { path: 'investments', component: MyInvestmentsComponent },
    { path: 'add-portfolio', component: AddPortfolioComponent },
    { path: 'edit-portfolio/:id', component: AddPortfolioComponent },
    { path: 'sync-exchange', component: SyncExchangeComponent },
    { path: 'exchange-portfolio/:id', component: ExchangePortfolioDetailsComponent },
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes)]
})

export class AppRoutingModule { }
