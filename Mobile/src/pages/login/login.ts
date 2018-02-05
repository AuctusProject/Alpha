import { Component, Injector } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { BasePage } from '../base';
import { TabsPage } from './../tabs/tabs';
import { ForgotPasswordPage } from './../forgot-password/forgot-password';

import { AccountService } from './../../services/account.service';

import { Login } from '../../models/login.model';


@Component({
    selector: 'page-login',
    templateUrl: 'login.html',
})
export class LoginPage extends BasePage {

    public login: Login;

    public loginForm: FormGroup;

    public forgotPasswordPage: any;

    constructor(protected injector: Injector, private accountService: AccountService) {
        super(injector);
        this.login = new Login();
        this.buildForm();
        this.forgotPasswordPage = ForgotPasswordPage;
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
                .subscribe(success => {
                    this.storageHelper.setLoginToken(success.jwt);
                    this.loadingHelper.hideLoading();
                    this.navCtrl.push(TabsPage);
                }, error => {
                    this.loadingHelper.hideLoading();
                    this.alertHelper.errorAlert(error)
                });
        }
    }
}
