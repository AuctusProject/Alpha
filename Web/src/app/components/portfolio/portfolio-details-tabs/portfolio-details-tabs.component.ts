import { Portfolio } from './../../../model/portfolio/portfolio';
import { Component, OnInit, Input } from '@angular/core';
import { Goal } from '../../../model/account/goal';

@Component({
  selector: 'portfolio-details-tabs',
  templateUrl: './portfolio-details-tabs.component.html',
  styleUrls: ['./portfolio-details-tabs.component.css']
})
export class PortfolioDetailsTabsComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  selectedIndex : number;
  
  constructor() { }

  ngOnInit() {
    this.selectedIndex = 0;
  }

  afterPurchaseCompleted(){
    this.selectedIndex = 2;
  }
}
