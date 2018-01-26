import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/account/login/login.component';
import { WizardComponent } from './components/register/wizard/wizard.component';
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { AdvisorsComponent } from './components/advisor/advisors/advisors.component';
import { AdvisorDetailComponent } from './components/advisor/advisor-detail/advisor-detail.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { HomeComponent } from "./components/home/home.component";
import { DashboardComponent } from "./components/dashboard/dashboard/dashboard.component";
import { ProjectionTabComponent } from "./components/dashboard/projection-tab/projection-tab.component";
import { PortfolioTabComponent } from "./components/dashboard/portfolio-tab/portfolio-tab.component";
import { HistoricalTabComponent } from "./components/dashboard/historical-tab/historical-tab.component";
import { ChangePasswordComponent } from './components/account/change-password/change-password.component';
import { ManageApiComponent } from './components/account/manage-api/manage-api.component';
import { AdvisorWizardComponent } from './components/account/advisor-wizard/advisor-wizard.component';


const routes: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'forgot-password-email', component: ForgotPasswordEmailComponent },
    { path: 'forgot-password-reset', component: ForgotPasswordResetComponent },
    { path: 'try', component: WizardComponent },
    { path: 'confirm', component: ConfirmEmailComponent },
    { path: 'advisors', component: AdvisorsComponent },
    { path: 'advisor/:id', component: AdvisorDetailComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'change-password', component: ChangePasswordComponent },
    { path: 'manage-api', component: ManageApiComponent },
    { path: 'become-advisor', component: AdvisorWizardComponent },
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes)]
})

export class AppRoutingModule { }
