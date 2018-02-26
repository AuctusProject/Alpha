import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'portfolio-projection',
  templateUrl: './portfolio-projection.component.html',
  styleUrls: ['./portfolio-projection.component.css']
})
export class PortfolioProjectionComponent implements OnInit {

  @Input() portfolio: Portfolio;

  constructor() {

  }

  ngOnInit() {
  }
}
