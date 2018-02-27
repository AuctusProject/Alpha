import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { Goal } from '../../../model/account/goal';
import * as moment from 'moment';
import Chart from 'chart.js';

@Component({
  selector: 'portfolio-projection',
  templateUrl: './portfolio-projection.component.html',
  styleUrls: ['./portfolio-projection.component.css']
})
export class PortfolioProjectionComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  @ViewChild("baseChart") baseChart: any;

  public startDate: Date;
  public endDate: Date;

  constructor() {
  }

  ngOnInit() {
    this.startDate = moment().startOf('date').toDate();
    this.endDate = moment().startOf('date').add(30, 'days').toDate();
    this.buildChart();
  }
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
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin'
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin'
    },{
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin'
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin'
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
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
            return '$' + label.toFixed(2);
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
          return moment(tooltipItems[0].xLabel).format('MMMM DD YYYY');
        },
        label: function (tooltipItems, data) {
          return data.datasets[tooltipItems.datasetIndex].label + ": $" + tooltipItems.yLabel.toFixed(2);
        }
      }
    },
    annotation: null
  };
  public chartType: string = 'line';

  public onEndDateChange(endDate: Date) {
    this.endDate = endDate;
    this.chartData = [];
    this.baseChart.datasets = [];
    this.baseChart.labels = [];
    console.log(this.baseChart);
    this.buildChart();
  }

  public buildChart() {

    this.buildChartLabels();
    //this.buildMonthyContributionChart();
    this.buildChartData(this.portfolio.pessimisticPercent, 'Pessimistic');
    this.buildChartData(this.portfolio.projectionPercent, 'Projection');
    this.buildChartData(this.portfolio.optimisticPercent, 'Optimistic');
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
    return this.getEstimatedDays() + 5;
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

    let chartData = [];
    if (monthPercent > 0) {
      let estimatedDays = this.getEstimatedDays();
      let extensionChartPoint = this.getExtensionChartPoint();

      var projectionValue = [];
      projectionValue.push(this.getStartingAmount());

      chartData.push(this.getStartingAmount());

      for (let day = 1; day <= extensionChartPoint; day++) {

        let beforeValue = projectionValue[day - 1];
        let currencyValue = beforeValue + (beforeValue * monthPercent / 30 / 100);

        //currencyValue += this.projection.currentGoal.monthlyContribution;
        projectionValue.push(currencyValue);
        chartData.push(currencyValue);
      }
    }

    this.chartData.push({ data: chartData, label: label });
  }
}
