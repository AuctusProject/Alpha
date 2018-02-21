import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from "../../../model/portfolio/portfolio";

@Component({
  selector: 'portfolio-card',
  templateUrl: './portfolio-card.component.html',
  styleUrls: ['./portfolio-card.component.css']
})
export class PortfolioCardComponent implements OnInit {
  @Input() portfolio: Portfolio;
  constructor() { }

  ngOnInit() {
  }

  
}
