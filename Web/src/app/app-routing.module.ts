import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { WizardComponent } from './components/register/wizard/wizard.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'try', component: WizardComponent }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)]
})

export class AppRoutingModule { }
