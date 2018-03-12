import { Component, OnInit, Input } from '@angular/core';
import { UserRank } from '../../../../model/account/userRank';

@Component({
  selector: 'user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {

  @Input() user: UserRank;

  constructor() { }

  ngOnInit() {
  }

}
