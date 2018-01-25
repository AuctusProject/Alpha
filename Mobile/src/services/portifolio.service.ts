
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';



@Injectable()
export class PortifolioService extends BaseService {

    private getDistributionUrl =  this.apiUrl("portfolios/v1/distribution");
    private projectionsUrl = this.apiUrl("portfolios/v1/projections");

    constructor(protected injector: Injector) {
        super(injector);
    }

    public getDistribution() {
        let params = new HttpParams();
        return this.httpGet(this.getDistributionUrl, params);
    }

    public getProjections() {
        let params = new HttpParams();
        return this.httpGet(this.projectionsUrl, params);
    }
}
