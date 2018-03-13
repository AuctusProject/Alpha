import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Advisor } from "../model/advisor/advisor";
import { AdvisorRank} from "../model/advisor/advisorRank"
import { Portfolio } from "../model/portfolio/portfolio";
import { Goal } from '../model/account/goal';
import { BuyRequest } from '../model/advisor/buyRequest';
import { CheckTransaction } from '../model/advisor/checkTransaction';

@Injectable()
export class AdvisorService {

  private baseGetAdvisorsUrl = this.httpService.apiUrl("advisors/v1");
  private baseAdvisorsPurchaseUrl = this.httpService.apiUrl("advisors/v1/purchases");
  private setBuyTransactionUrl = this.httpService.apiUrl("advisors/v1/purchases/{0}/transaction");
  private getAdvisorsRankUrl = this.httpService.apiUrl("/advisors/v1/rank");
  private checkBuyTransactionUrl = this.httpService.apiUrl("advisors/v1/purchases/{0}/check");
  
  
  constructor(private httpService : HttpService) { }

  getAdvisor(id: string): Observable<Advisor> {
    return this.httpService.get(this.baseGetAdvisorsUrl + "/" + id);
  }

  getAdvisorsRank(): Observable<AdvisorRank[]> {
    return this.httpService.get(this.getAdvisorsRankUrl);
  }
  
  createAdvisor(advisor: Advisor): Observable<Advisor> {
    return this.httpService.post(this.baseGetAdvisorsUrl, advisor);
  }

  buy(buyRequest : BuyRequest): Observable<any> {
    return this.httpService.post(this.baseAdvisorsPurchaseUrl, buyRequest);
  }

  cancelBuyTransaction(buyId : number): Observable<any> {
    return this.httpService.delete(this.baseAdvisorsPurchaseUrl + "/" + buyId);
  }
  
  setBuyTransaction(buyId, hash : string): Observable<string> {
    var setBuyRequest = {
      transactionHash: hash
    };
    return this.httpService.post(this.setBuyTransactionUrl.replace("{0}", buyId), setBuyRequest);
  }

  checkBuyTransaction(buyId): Observable<CheckTransaction> {
    return this.httpService.post(this.checkBuyTransactionUrl.replace("{0}", buyId));
  }

  updateAdvisor(advisor: Advisor): Observable<void> {
    return this.httpService.patch(this.baseGetAdvisorsUrl + "/" + advisor.id, advisor);
  }
}
