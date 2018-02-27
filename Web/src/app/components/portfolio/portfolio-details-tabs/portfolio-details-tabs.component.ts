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
  
  constructor() { }

  ngOnInit() {
  }

}
