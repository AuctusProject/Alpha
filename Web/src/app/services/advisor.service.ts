import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advisor/advisor";
import { Portfolio } from "../model/portfolio/portfolio";
import { Goal } from '../model/account/goal';
import { BuyRequest } from '../model/advisor/buyRequest';

@Injectable()
export class AdvisorService {

  private baseGetAdvisorsUrl = this.httpService.apiUrl("advisors/v1");
  private baseAdvisorsPurchaseUrl = this.httpService.apiUrl("advisors/v1/purchases");
  private setBuyTransactionUrl = this.httpService.apiUrl("advisors/v1/purchases/{0}/transaction");

  
  constructor(private httpService : HttpService) { }

  getAdvisor(id: string): Observable<Advisor> {
    return this.httpService.get(this.baseGetAdvisorsUrl + "/" + id);
  }

  createAdvisor(advisor: Advisor): Observable<Advisor> {
    return this.httpService.post(this.baseGetAdvisorsUrl, advisor);
  }

  buy(buyRequest : BuyRequest): Observable<any> {
    return this.httpService.post(this.baseAdvisorsPurchaseUrl, buyRequest);
  }
  
  setBuyTransaction(buyId, hash : string): Observable<string> {
    var setBuyRequest = {
      transactionHash: hash
    };
    return this.httpService.post(this.setBuyTransactionUrl.replace("{0}", buyId), setBuyRequest);
  }

  updateAdvisor(advisor: Advisor): Observable<void> {
    return this.httpService.patch(this.baseGetAdvisorsUrl + "/" + advisor.id, advisor);
  }
}
