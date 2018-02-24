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
  
  public simulator =  {
    price: null,
    estimatedReturn: null,
    startDate: null,
    endDate: null,
    totalEstimatedReturn: null,
    totalPrice: null
  }

  public periodDescription: string;

  constructor() { }

  ngOnInit() {
    this.simulator.price = this.portfolio.price;
    this.simulator.estimatedReturn = this.portfolio.projectionPercent;
    this.simulator.startDate = moment().startOf('date').toDate();
    this.simulator.endDate = moment().startOf('date').add(1, 'month').toDate();

    this.calculatePeriod();

  }

  public onEndDateChange(){
    this.calculatePeriod();
    this.calculateTotalEstimatedReturn();
    this.calculateTotalPrice();
  }

  public calculatePeriod() {

    this.periodDescription = '';
    if(this.simulator.endDate) {
      let startDate = moment(this.simulator.startDate).startOf('date');
      let endDate = moment(this.simulator.endDate).startOf('date');

      let months = endDate.diff(startDate, 'month', true);
      if(months < 1){
        let days = endDate.diff(startDate, 'day');
        this.periodDescription = days + (days > 1 ? " days" : " day");
      } else {
        this.periodDescription += Math.floor(months) +  (Math.floor(months) > 1 ? " months" : " month");
        if(!(Number(months) === months && months % 1 === 0)){
          let auxDate = endDate.add('month', -Math.floor(months));
          let days = auxDate.diff(startDate, 'days');
          this.periodDescription += ' / ' + days + (days > 1 ? " days" : " day");
        }
      }
    } else {
      this.periodDescription = '0 days';
    }

    
  }

  public calculateTotalEstimatedReturn() {
    if(this.simulator.endDate) {
      let startDate = moment(this.simulator.startDate)
    }

    return '';
  }

  public calculateTotalPrice() {
    if(this.simulator.endDate) {
      let startDate = moment(this.simulator.startDate)
    }

    return 0;
  }

  

  public onBuyClick() {
    console.log(this.simulator);
  }
}
