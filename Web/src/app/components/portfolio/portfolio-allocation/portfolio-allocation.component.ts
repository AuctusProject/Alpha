import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'portfolio-allocation',
  templateUrl: './portfolio-allocation.component.html',
  styleUrls: ['./portfolio-allocation.component.css']
})
export class PortfolioAllocationComponent implements OnInit {

  @Input() portfolio: Portfolio;
  
  constructor() { }

  ngOnInit() {
  }

}
