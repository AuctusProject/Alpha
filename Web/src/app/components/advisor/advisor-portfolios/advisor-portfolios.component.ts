import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from "../../../model/portfolio/portfolio"

@Component({
  selector: 'advisor-portfolios',
  templateUrl: './advisor-portfolios.component.html',
  styleUrls: ['./advisor-portfolios.component.css']
})
export class AdvisorPortfoliosComponent implements OnInit {
  @Input() portfolios : Portfolio[];

  constructor() { }

  ngOnInit() {
  }

}
