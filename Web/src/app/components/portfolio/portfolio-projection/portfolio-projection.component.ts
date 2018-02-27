import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { Goal } from '../../../model/account/goal';

@Component({
  selector: 'portfolio-projection',
  templateUrl: './portfolio-projection.component.html',
  styleUrls: ['./portfolio-projection.component.css']
})
export class PortfolioProjectionComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;

  constructor() {

  }

  ngOnInit() {
  }
}
