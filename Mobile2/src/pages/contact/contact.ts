import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';


import * as Chartist from 'chartist';

import {
  ChartType,
  ChartEvent
} from 'angular2-chartist';

declare var require: any;

const data: any = require('../../assets/data.json');

export interface Chart {
  type: ChartType;
  data: Chartist.IChartistData;
  options?: any;
  responsiveOptions?: any;
  events?: ChartEvent;
}

@Component({
  selector: 'page-contact',
  templateUrl: 'contact.html'
})

export class ContactPage {

  public charts: Chart[];

  constructor(public navCtrl: NavController) {

  }

  ionViewDidEnter() {
    console.log(data);

    this.charts = [{
      type: 'Bar',
      data: data['Bar']
    }, {
      type: 'Line',
      data: data['Line']
    }, {
      type: 'Line',
      data: data['Line2']
    }, {
      type: 'Line',
      data: data['Scatter'],
      options: {
        showLine: false,
        axisX: {
          labelInterpolationFnc: function (value: number, index: number): string {
            return index % 13 === 0 ? `W${value}` : null;
          }
        }
      },
      responsiveOptions: [
        ['screen and (min-width: 640px)', {
          axisX: {
            labelInterpolationFnc: function (value: number, index: number): string {
              return index % 4 === 0 ? `W${value}` : null;
            }
          }
        }]
      ]
    }, {
      type: 'Line',
      data: data['LineWithArea'],
      options: {
        low: 0,
        showArea: true
      }
    }, {
      type: 'Bar',
      data: data['Bi-PolarBar'],
      options: {
        high: 10,
        low: -10,
        axisX: {
          labelInterpolationFnc: function (value: number, index: number): number {
            return index % 2 === 0 ? value : null;
          }
        }
      }
    }, {
      type: 'Bar',
      data: data['DistributedSeries'],
      options: {
        distributeSeries: true
      }
    }, {
      type: 'Pie',
      data: data['Pie'],
      options: {
        donut: true,
        donutWidth: 60,
        startAngle: 270,
        total: 200,
        showLabel: false
      }
    }, {
      type: 'Pie',
      data: data['Pie'],
      options: {
        donut: true,
        showLabel: false
      }
      // events: {
      //   draw(data: any): boolean {
      //     return data;
      //   }
      // }
    }];
  }

}
