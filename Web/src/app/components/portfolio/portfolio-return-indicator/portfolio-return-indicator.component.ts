import { PortfolioService } from './../../../services/portfolio.service';
import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'portfolio-return-indicator',
  templateUrl: './portfolio-return-indicator.component.html',
  styleUrls: ['./portfolio-return-indicator.component.css']
})
export class PortfolioReturnIndicatorComponent implements OnInit {

  @Input() estimatedReturn: any;
  @Input() realReturn: any;
  @Input() successRate: any;
  @Input() risk: any;

  constructor() { }

  ngOnInit() {    
  }

  getReturnClass() {
    let classBase = "text-rigth";
    if(this.realReturn > 0) {
      return classBase + " success-value";
    } else if(this.realReturn < 0) {
      return classBase + " fail-value";
    }
    return classBase;
  }
}
