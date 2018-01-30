
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';



@Injectable()
export class PortfolioService extends BaseService {

    private getDistributionUrl = this.apiUrl("portfolios/v1/distribution");
    private getHistoryUrl = this.apiUrl("portfolios/v1/history");
    private getProjectionsUrl = this.apiUrl("portfolios/v1/projections");

    constructor(protected injector: Injector) {
        super(injector);
    }

    public getDistribution() {
        let params = new HttpParams();
        return this.httpGet(this.getDistributionUrl, params);
    }

    public getProjection() {
        let params = new HttpParams();
        return this.httpGet(this.getProjectionsUrl, params);
    }

    public getHistory() {
        let params = new HttpParams();
        return this.httpGet(this.getHistoryUrl, params);
    }
}
