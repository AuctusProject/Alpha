import { Portfolio } from './../../../model/portfolio/portfolio';
import { Component, OnInit, Input } from '@angular/core';
import * as moment from 'moment';

import Chart from 'chart.js';
//import 'chartjs-plugin-annotation';

@Component({
  selector: 'portfolio-projection-chart',
  templateUrl: './portfolio-projection-chart.component.html',
  styleUrls: ['./portfolio-projection-chart.component.css']
})
export class PortfolioProjectionChartComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() startDate: Date;
  @Input() endDate: Date;

  constructor() { }

  public chartColors: Array<any> = [
    {
      //Contributed
      backgroundColor: 'rgb(74, 144, 226)',
      borderColor: 'rgb(74, 144, 226)',
      fill: 'false'
    },
    {
      //Pessimistic
      backgroundColor: 'rgb(188, 255, 248)',
      borderColor: 'rgb(188, 255, 248)',
      fill: 'origin'
    },
    {
      //Projection
      backgroundColor: 'rgb(97, 237, 223)',
      borderColor: 'rgb(97, 237, 223)',
      fill: 'origin'
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin'
    }
  ];
  public chartData: Array<any> = new Array<any>();
  public chartLabels: Array<string> = new Array<string>();
  public chartLegend: boolean = true;
  public chartOptions: any = {
    responsive: true,
    scales: {
      xAxes: [{
        drawBorder: true,
        drawOnChartArea: false,
        display: true,
        type: 'time',
        ticks: {
          fontFamily: 'HelveticaNeueMedium',
        },
      }],
      yAxes: [{
        drawBorder: false,
        color: ['#FF0000'],
        ticks: {
          fontFamily: 'HelveticaNeueMedium',
          callback: function (label, index, labels) {
            return PortfolioProjectionChartComponent.formatCurrency(label, '$');
          }
        }
      }]
    },
    legend: {
      display: false
    },
    tooltips: {
      enabled: true,
      mode: 'single',
      callbacks: {
        title: function (tooltipItems, data) {
          return moment(tooltipItems[0].xLabel).format('MMMM YYYY');
        },
        label: function (tooltipItems, data) {
          let formatValue = PortfolioProjectionChartComponent.formatCurrency(tooltipItems.yLabel, '$');
          return data.datasets[tooltipItems.datasetIndex].label + ":" + formatValue;
        }
      }
    },
    annotation: null
  };
  public chartType: string = 'line';

  private static formatCurrency(valueToFormat: any, currency: string): string {
    let value = Number(valueToFormat);
    if (value >= 1000000) {
      return currency + (value / 1000000).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'M'
    } else if (value > 1000) {
      return currency + (value / 1000).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'k'
    } else {
      return currency + value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }
  }

  ngOnInit() {
    this.buildChart();
  }

  private buildChart() {
    this.buildChartLabels();
    //this.buildMonthyContributionChart();
    //this.buildChartData(this.portfolio.pessimisticPercent, 'Pessimistic');
    this.buildChartData(this.portfolio.projectionPercent, 'Projection');
   // this.buildChartData(this.portfolio.optimisticPercent, 'Optimistic');
    this.buildTargetLine();
  }


  private buildTargetLine() {

    let endDate = this.getEndDate();

    let annotation = {
      drawTime: 'afterDatasetsDraw',
      annotations: [{
        id: 'vline',
        type: 'line',
        mode: 'vertical',
        scaleID: 'x-axis-0',
        value: endDate.format('YYYY-MM-DD'),
        borderColor: '#002d78',
        borderWidth: 2,
        label: {
          backgroundColor: '#002d78',
          content: 'Target',// - ' + goalDate.format('MMM YYYY'),
          enabled: true,
          position: 'top'
        },
      }]
    }

    this.chartOptions.annotation = annotation;
  }

  private getStartDate() {
    return moment(this.startDate).startOf('date');
  }

  private getEndDate() {
    return moment(this.endDate).startOf('date');
  }

  private getStartingAmount() {
    return 100;
  }

  private getEstimatedDays() {

    let startDate = this.getStartDate();
    let endDate = this.getEndDate();

    return endDate.diff(startDate, 'days');
  }

  private getExtensionChartPoint() {
    return Math.round(this.getEstimatedDays() * 1.2);
  }

  private buildChartLabels() {

    let startDate = this.getStartDate();
    let estimatedDays = this.getEstimatedDays();
    let extensionChartPoint = this.getExtensionChartPoint();

    let chartLabels = [];
    chartLabels.push(startDate.toDate());

    for (let day = 1; day <= extensionChartPoint; day++) {

      let date = startDate.add('day', 1).toDate();
      chartLabels.push(date);
      
    }

    this.chartLabels = chartLabels;
  }

  private buildChartData(monthPercent, label) {

    let estimatedDays = this.getEstimatedDays();
    let extensionChartPoint = this.getExtensionChartPoint();

    var projectionValue = [];
    projectionValue.push(this.getStartingAmount());

    let chartData = [];
    chartData.push(this.getStartingAmount());

    for (let day = 1; day <= extensionChartPoint; day++) {

      let beforeValue = projectionValue[day - 1];
      let currencyValue = beforeValue + (beforeValue * monthPercent / 30 / 100);

      //currencyValue += this.projection.currentGoal.monthlyContribution;
      projectionValue.push(currencyValue);
      chartData.push(currencyValue);
    }

    this.chartData.push({ data: chartData, label: label })
  }
}
