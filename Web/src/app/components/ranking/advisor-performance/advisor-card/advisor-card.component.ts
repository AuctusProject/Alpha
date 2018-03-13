import { Component, OnInit, Input } from '@angular/core';
import { AdvisorRank } from '../../../../model/advisor/advisorRank';

@Component({
  selector: 'advisor-card',
  templateUrl: './advisor-card.component.html',
  styleUrls: ['./advisor-card.component.css']
})
export class AdvisorCardComponent implements OnInit {

  @Input() advisor: AdvisorRank;
  @Input() position: number;

  constructor() { }

  ngOnInit() {
  }

}
