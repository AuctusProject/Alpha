
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Login } from '../models/login.model';

@Injectable()
export class AccountService extends BaseService {

    private loginUrl = this.apiUrl("account/login");

    constructor(protected injector: Injector) {
        super(injector);
    }

    public login(login: Login) {
        let params = new HttpParams();
        return this.httpPost(this.loginUrl, login, params);
    }
}
