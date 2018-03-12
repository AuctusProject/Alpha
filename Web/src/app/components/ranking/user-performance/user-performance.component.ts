import { AccountService } from './../../../services/account.service';
import { Component, OnInit } from '@angular/core';
import { UserBalance } from '../../../model/account/userBalance';
import { UserRank } from '../../../model/account/userRank';
import { MatTabChangeEvent } from '@angular/material';

@Component({
  selector: 'app-user-performance',
  templateUrl: './user-performance.component.html',
  styleUrls: ['./user-performance.component.css']
})
export class UserPerformanceComponent implements OnInit {

  public dailyList: Array<UserRank>;
  public allTimeList: Array<UserRank>;

  public searchFields = {
    name: null,
    date: null
  };

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.listUsersByPerformance();
  }

  public listUsersByPerformance() {
    this.accountService.listUsersByPerformance().subscribe(result => {
      this.allTimeList = result
    });
  };

  public onTabChange(event: MatTabChangeEvent) {
    if (event.index === 1 && !this.searchFields.date) {
        this.searchFields.date = new Date();
        this.listUsersPerformanceByDate();
    }
  }

  public listUsersPerformanceByDate() {
    this.dailyList = null;
    this.accountService.listUsersPerformanceByDate(this.searchFields.date).subscribe(result => {
      this.dailyList = result
    });
  }

}
