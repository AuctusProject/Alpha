import { Component, Injector } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

import { BasePage } from './../base';
import { AccountService } from '../../services/account.service';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'page-forgot-password',
  templateUrl: 'forgot-password.html',
})
export class ForgotPasswordPage extends BasePage {


  public forgotPasswordForm: FormGroup;
  public emailAccount: string;
  public forgotPassword = {
    email: ''
  };

  constructor(protected injector: Injector, private accountService: AccountService) {
    super(injector);
        this.buildForm();
  }

  private buildForm() {
    this.forgotPasswordForm = this.formBuilder.group({
        email: ['', this.validators.compose([this.validators.email, this.validators.required])]
    });
}


public onSubmit() {

    this.isSubmittedForm = true;

    if (this.forgotPasswordForm.valid) {
        this.loadingHelper.showLoading();
        this.accountService.forgotPassword(this.forgotPassword.email)
            .subscribe(success => {
                
            }, error => {
                
            });
    }
}

}
