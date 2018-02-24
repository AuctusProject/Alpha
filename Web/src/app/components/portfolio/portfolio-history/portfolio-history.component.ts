import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'portfolio-history',
  templateUrl: './portfolio-history.component.html',
  styleUrls: ['./portfolio-history.component.css']
})
export class PortfolioHistoryComponent implements OnInit {

  @Input() portfolio: Portfolio;
  
  constructor() { }

  ngOnInit() {
  }

}
