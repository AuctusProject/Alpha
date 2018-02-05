
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Login } from '../models/login.model';

@Injectable()
export class AccountService extends BaseService {

    private loginUrl = this.apiUrl("accounts/v1/login");
    private forgotPasswordUrl = this.apiUrl("accounts/v1/password/forgotten");

    constructor(protected injector: Injector) {
        super(injector);
    }

    public login(login: Login) {
        let params = new HttpParams();
        return this.httpPost(this.loginUrl, login, params);
    }

    public forgotPassword(email: string) {
        let params = new HttpParams();
        let forgotPassword = {
            Email: email
          }
        return this.httpPost(this.forgotPasswordUrl, forgotPassword, params);
    }
}
