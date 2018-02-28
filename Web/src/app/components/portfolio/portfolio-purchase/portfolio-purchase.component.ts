import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { MetamaskAccountService } from "../../../services/metamask-account.service";
import { AdvisorService } from "../../../services/advisor.service";
import * as moment from 'moment';
import { BuyRequest } from '../../../model/advisor/buyRequest';
import { DateUtil } from "../../../util/dateUtil";
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';
import { Goal } from '../../../model/account/goal';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'portfolio-purchase',
  templateUrl: './portfolio-purchase.component.html',
  styleUrls: ['./portfolio-purchase.component.css']
})
export class PortfolioPurchaseComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  @Output() afterEndDateChange = new EventEmitter();
  loginData: any;
  startDate: Date;
  endDate: Date;

  public simulator = {
    price: null,
    estimatedReturn: null,
    startDate: null,
    endDate: null,
    months: 0,
    days: 0,
    estimatedTotalReturn: 0,
    totalPrice: 0
  }

  public transactionUrl: string;
  public purchasePromise: Subscription;
  public timeDescription: string;

  constructor(private metamaskAccount: MetamaskAccountService, private advisorService: AdvisorService, private loginService: LoginService) { }

  ngOnInit() {

    this.startDate = moment().startOf('date').toDate();

    let totalDays = this.goal != null ? this.goal.timeframe * 12 * 30 : 30;
    this.endDate = moment().startOf('date').add(totalDays, 'days').toDate();

    this.simulator.price = this.portfolio.price;
    this.simulator.estimatedReturn = this.portfolio.projectionPercent;
    this.simulator.startDate = this.startDate;
    this.simulator.endDate = this.endDate;

    this.updateSimulator();

    let logged = this.loginService.isLoggedIn();
    if (logged) {
      this.loginData = this.loginService.getLoginData();
    }
  }

  public getStartDate() {
    return this.simulator.startDate;
  }

  public getEndDate() {
    return this.simulator.endDate;
  }

  public updateSimulator() {
    this.calculateTime();
    this.setTimeDescription();
    this.calculateSimulator();
  }

  public onEndDateChange() {

    this.updateSimulator()
    this.emitEndDateEvent();
  }

  private emitEndDateEvent() {
    if (this.afterEndDateChange) {
      this.afterEndDateChange.emit(this.simulator.endDate);
    }
  }

  private calculateTime() {

    this.simulator.months = 0;
    this.simulator.days = 0;

    if (this.simulator.endDate) {

      let startDate = moment(this.simulator.startDate).startOf('date');
      let endDate = moment(this.simulator.endDate).startOf('date');

      let totalDays = endDate.diff(startDate, 'day');

      this.simulator.months = Math.floor(totalDays / 30);
      this.simulator.days = totalDays - this.simulator.months * 30;
    }
  }

  public setTimeDescription() {

    this.timeDescription = "";

    if (this.simulator.months > 0) {
      this.timeDescription += this.simulator.months + (this.simulator.months > 1 ? " months " : " month ");
    }

    if (this.simulator.days > 0) {
      this.timeDescription += this.simulator.days + (this.simulator.days > 1 ? " days" : " day");
    }
  }

  public calculateSimulator() {
    this.simulator.estimatedTotalReturn = 0;
    this.simulator.totalPrice = 0;

    if (this.simulator.endDate) {

      let months = this.simulator.months + this.simulator.days / 30;

      this.simulator.estimatedTotalReturn = this.simulator.estimatedReturn * months;
      this.simulator.totalPrice = this.simulator.price * months;
    }
  }

  public onBuyClick() {
    var self = this;
    var days = DateUtil.DiffDays(this.simulator.startDate, this.simulator.endDate);
    this.purchasePromise = new Observable(observer => {
      this.advisorService.buy(new BuyRequest(this.portfolio.id, days,
        this.metamaskAccount.getAccount(), this.goal)).subscribe(
          result => {
            if (result) {
              var id = result.id;
              this.metamaskAccount.sendAUC(this.simulator.totalPrice).subscribe(
                hash => {
                  if (hash) {
                    this.advisorService.setBuyTransaction(id, hash).subscribe(
                      success => {
                        if (success) {
                          this.portfolio.pendingConfirmation = true;
                          this.transactionUrl = environment.etherscanUrl + "/tx/" + hash;
                          observer.next(true);
                        }
                        else {
                          observer.complete();
                        }
                      }
                    )
                  }
                  else {
                    this.advisorService.cancelBuyTransaction(id).subscribe(result => observer.complete());
                  }
                })
            }
            else {
              observer.complete();
            }
          })
    }).subscribe();
  }

}
