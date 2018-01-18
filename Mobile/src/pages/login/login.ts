import { TabsPage } from './../tabs/tabs';
import { AccountService } from './../../services/account.service';
import { Component, Injector } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

import { FormGroup } from '@angular/forms';
import { BasePage } from '../base';
import { Login } from '../../models/login.model';


@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})
export class LoginPage extends BasePage {

  public login: Login;

  public loginForm: FormGroup;

  constructor(protected injector: Injector, private accountService: AccountService) {

    super(injector);

    this.login = new Login();

    this.buildForm();

  }

  private buildForm() {
    this.loginForm = this.formBuilder.group({
      email: ['', this.validators.compose([this.validators.email, this.validators.required])],
      password: ['', this.validators.compose([this.validators.required])],
    });
  }



  public onSubmit() {

    this.isSubmittedForm = true;

    if (this.loginForm.valid) {
      this.loadingHelper.showLoading();
      this.accountService.login(this.login)
        .subscribe(success.bind(this), error.bind(this));
    }

    function success(response) {
      this.storageHelper.setLoginToken(response.jwt);
      this.loadingHelper.hideLoading();
      this.navCtrl.push(TabsPage);
    }

    function error(response) {
      this.loadingHelper.hideLoading();
      this.alertHelper.errorAlert(response)
    }
  }
}
