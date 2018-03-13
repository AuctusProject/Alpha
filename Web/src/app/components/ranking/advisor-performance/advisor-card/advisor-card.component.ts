import { Component, OnInit, Input } from '@angular/core';
import { AdvisorRank } from '../../../../model/account/AdvisorRank';

@Component({
  selector: 'advisor-card',
  templateUrl: './advisor-card.component.html',
  styleUrls: ['./advisor-card.component.css']
})
export class AdvisorCardComponent implements OnInit {

  @Input() user: AdvisorRank;

  constructor() { }

  ngOnInit() {
  }

}
