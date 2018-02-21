import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'risk-indicator',
  templateUrl: './risk-indicator.component.html',
  styleUrls: ['./risk-indicator.component.css']
})
export class RiskIndicatorComponent implements OnInit {
  @Input() risk : number;
  possibleRisks = [1,2,3,4,5];
  constructor() { }

  ngOnInit() {
  }

  private getRiskDescription() {
    if (this.risk == 1) {
      return "Conservative";
    }
    else if (this.risk == 2) {
      return "Moderately Conservative";
    }
    else if (this.risk == 3) {
      return "Moderately Aggressive";
    }
    else if (this.risk == 4) {
      return "Aggressive";
    }
    else if (this.risk == 5) {
      return "Very Aggressive"
    }
  }
}
