import { Component } from '@angular/core';

import { ChartType } from 'angular2-chartist';

import * as Chartist from 'chartist';

declare var require: any;

const data: any = require('../../assets/data.json');

@Component({
  selector: 'async-chart',
  template: `
    <h4>Async</h4>
    <x-chartist
      [data]="data"
      [type]="type">
    </x-chartist>
  `
})
class AsyncChartComponent {
  data: Promise<Chartist.IChartistData>;
  type: Promise<ChartType>;

  constructor() {
    // simulate slow API call
    this.data = new Promise(function(resolve: any): void {
      setTimeout(function(): void {
        resolve(data['Pie']);
      }, 5000);
    });

    this.type = new Promise(function(resolve: any): void {
      setTimeout(function(): void {
        resolve('Pie');
      }, 5000);
    });
  }
}

export { AsyncChartComponent };