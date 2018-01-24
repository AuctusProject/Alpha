
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Login } from '../models/login.model';


@Injectable()
export class AdviceService extends BaseService {

    private projectionsUrl = this.apiUrl("advice/projections");

    constructor(protected injector: Injector) {
        super(injector);
    }

    public getProjections() {
        let params = new HttpParams();
        return this.httpGet(this.projectionsUrl, params);
    }
}
