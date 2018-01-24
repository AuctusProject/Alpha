
import { HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

@Injectable()
export class AdviceService extends BaseService {

    constructor(protected injector: Injector) {
        super(injector);
    }

    
}
