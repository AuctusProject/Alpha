import { AccountService } from './../../../services/account.service';
import { Component, OnInit } from '@angular/core';
import { UserBalance } from '../../../model/account/userBalance';
import { UserRank } from '../../../model/account/userRank';
import { MatTabChangeEvent } from '@angular/material';
import * as moment from 'moment';

@Component({
  selector: 'app-user-performance',
  templateUrl: './user-performance.component.html',
  styleUrls: ['./user-performance.component.css']
})
export class UserPerformanceComponent implements OnInit {

  public dailyList: Array<UserRank>;
  public allTimeList: Array<UserRank>;

  public maxDate = moment().add(-1, 'days').toDate();

  public searchFields = {
    name: null,
    date: null
  };

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.searchFields.date = this.maxDate;
    this.listUsersPerformanceByDate();
  }

  public listUsersByPerformance() {
    this.accountService.listUsersByPerformance().subscribe(result => {
      this.allTimeList = result
    });
  };

  public onTabChange(event: MatTabChangeEvent) {
    if (event.index === 0) {
        this.listUsersByPerformance();
    }
  }

  public listUsersPerformanceByDate() {
    this.dailyList = null;
    this.accountService.listUsersPerformanceByDate(this.searchFields.date).subscribe(result => {
      this.dailyList = result
    });
  }

}
