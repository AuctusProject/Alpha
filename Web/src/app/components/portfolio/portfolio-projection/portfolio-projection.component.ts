import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import * as moment from 'moment';

@Component({
  selector: 'portfolio-projection',
  templateUrl: './portfolio-projection.component.html',
  styleUrls: ['./portfolio-projection.component.css']
})
export class PortfolioProjectionComponent implements OnInit {

  @Input() portfolio: Portfolio;

  public simulator = {
    price: null,
    estimatedReturn: null,
    startDate: null,
    endDate: null,
    years: 0,
    months: 0,
    days: 0,
    estimatedTotalReturn: 0,
    totalPrice: 0
  }

  public timeDescription: string;

  constructor() { }

  ngOnInit() {
    this.simulator.price = this.portfolio.price;
    this.simulator.estimatedReturn = this.portfolio.projectionPercent;
    this.simulator.startDate = moment().startOf('date').toDate();
    this.simulator.endDate = moment().startOf('date').add(1, 'month').toDate();

    this.updateSimulator();
  }

  public updateSimulator() {
    this.calculateTime();
    this.setTimeDescription();
    this.calculateSimulator();
  }

  private calculateTime() {

    this.simulator.years = 0;
    this.simulator.months = 0;
    this.simulator.days = 0;

    if (this.simulator.endDate) {

      let startDate = moment(this.simulator.startDate).startOf('date');
      let endDate = moment(this.simulator.endDate).startOf('date');

      this.simulator.years = Math.floor(endDate.diff(startDate, 'year', true));
      this.simulator.months = endDate.diff(startDate, 'month', true) - (this.simulator.years * 12);

      if (this.simulator.months < 1) {
        this.simulator.days = endDate.diff(startDate, 'day') - (this.simulator.years * 365);
      } else if (this.simulator.months > Math.floor(this.simulator.months)) {

        let auxDate = moment(this.simulator.startDate).add(Math.floor(this.simulator.months), 'month').startOf('date');
        this.simulator.days = endDate.diff(auxDate, 'days') - (this.simulator.years * 365);

        if (this.simulator.days > 29) {
          this.simulator.months += 1;
          this.simulator.days = 0;
        }

      }

      this.simulator.months = Math.floor(this.simulator.months);
    }
  }

  public setTimeDescription() {

    this.timeDescription = "";
    
    if (this.simulator.years > 0) {
      this.timeDescription += this.simulator.years + (this.simulator.years > 1 ? " years " : " year ");
    }

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

      let months = this.simulator.years * 12 + this.simulator.months + this.simulator.days / 30;

      this.simulator.estimatedTotalReturn = this.simulator.estimatedReturn * months;
      this.simulator.totalPrice = this.simulator.price * months;
    }
  }



  public onBuyClick() {
    console.log(this.simulator);
  }
}
