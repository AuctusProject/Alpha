import { Component, OnInit, Input } from '@angular/core';
import { RiskType } from '../../../../model/account/riskType'

@Component({
  selector: 'portfolio-register',
  templateUrl: './portfolio-register.component.html',
  styleUrls: ['./portfolio-register.component.css']
})
export class PortfolioRegisterComponent implements OnInit {
  riskDescription = "Conservative";
  @Input() risk: RiskType;

  isEditing = false;

  constructor() { }

  ngOnInit() {
  }

  public addPortfolio() {
    this.isEditing = true;
  }

}
