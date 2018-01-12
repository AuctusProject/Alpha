import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/account/login/login.component';
import { WizardComponent } from './components/register/wizard/wizard.component';
import { ForgotPasswordEmailComponent } from './components/account/forgot-password-email/forgot-password-email.component';
import { ForgotPasswordResetComponent } from './components/account/forgot-password-reset/forgot-password-reset.component';
import { AdvisorsComponent } from './components/advice/advisors/advisors.component';
import { AdvisorDetailComponent } from './components/advice/advisor-detail/advisor-detail.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'forgot-password-email', component: ForgotPasswordEmailComponent },
    { path: 'forgot-password-reset', component: ForgotPasswordResetComponent },
    { path: 'try', component: WizardComponent },
    { path: 'confirm', component: ConfirmEmailComponent },
    { path: 'advisors', component: AdvisorsComponent },
    { path: 'advisor/:id', component: AdvisorDetailComponent }];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes)]
})

export class AppRoutingModule { }
