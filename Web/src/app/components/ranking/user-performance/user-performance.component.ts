import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-performance',
  templateUrl: './user-performance.component.html',
  styleUrls: ['./user-performance.component.css']
})
export class UserPerformanceComponent implements OnInit {

  public usersList: Array<any>;

  public searchFields = {
    name: null,
    date: new Date()
  };

  constructor() { }

  ngOnInit() {
    this.usersList =[
      {name: 'Renzo', rank: 1, totalValue: 100000, percentageValue: 120},
      {name: 'Jose', rank: 2, totalValue: 10000, percentageValue: 120},
      {name: 'Maria', rank: 3, totalValue: 1000, percentageValue: 120},
      {name: 'TEste', rank: 4, totalValue: 100, percentageValue: 120}
    ]
  }

}
