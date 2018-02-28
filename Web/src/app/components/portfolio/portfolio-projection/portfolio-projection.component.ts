import { Component, OnInit, Input, ViewChild, AfterViewInit } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { PortfolioPurchaseComponent } from '../portfolio-purchase/portfolio-purchase.component';
import { Goal } from '../../../model/account/goal';
import * as moment from 'moment';
import Chart from 'chart.js';
import 'chartjs-plugin-annotation';

@Component({
  selector: 'portfolio-projection',
  templateUrl: './portfolio-projection.component.html',
  styleUrls: ['./portfolio-projection.component.css']
})
export class PortfolioProjectionComponent implements OnInit {

  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  @ViewChild("baseChart") baseChart: any;
  @ViewChild(PortfolioPurchaseComponent) portfolioPurchaseComponent;

  constructor() {
  }

  ngOnInit() {
    
  }

  ngAfterViewInit() {
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
      fill: 'origin',
    },
    {
      //Projection
      backgroundColor: 'rgb(97, 237, 223)',
      borderColor: 'rgb(97, 237, 223)',
      fill: 'origin',
    },
    {
      //Optimistic
      backgroundColor: "rgb(15, 215, 185)",
      borderColor: "rgb(15, 215, 185)",
      fill: 'origin',
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
        gridLines: { borderDash: [3], borderDashOffset: [15], drawBorder: false, color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb'] },
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

  public onEndDateChange() {
    this.chartData = [];
    this.buildChart();
    this.baseChart.refresh();
  }

  public buildChart() {

    this.buildChartLabels();
    this.buildMonthyContributionChart();
    this.buildChartData(this.portfolio.pessimisticPercent, 'Pessimistic');
    this.buildChartData(this.portfolio.projectionPercent, 'Projection');
    this.buildChartData(this.portfolio.optimisticPercent, 'Optimistic');
    this.buildTargetLines();
  }

  private buildMonthyContributionChart() {

    let chartData = [];

    if (this.goal != null && this.goal.monthlyContribution > 0) {
      let estimatedDays = this.getEstimatedDays();

      var projectionValue = [];
      projectionValue.push(this.getStartingAmount());

      let lastChartValue = this.getStartingAmount();
      chartData.push(lastChartValue);

      let extensionChartPoint = this.getExtensionChartPoint();

      for (let day = 1; day <= extensionChartPoint; day++) {

        let beforeValue = projectionValue[day - 1];
        let currencyValue = beforeValue + this.goal.monthlyContribution / 30;
        projectionValue.push(currencyValue);

        if (day % this.getChartDaysInterval() == 0 || day == estimatedDays) {
          lastChartValue = day % 30 == 0 ? currencyValue : lastChartValue;
          chartData.push(lastChartValue);
        }
      }

    }

    this.chartData.push({ data: chartData, label: 'Contributed' })
  }

  private buildTargetLines() {

    let annotationsList = [];

    let endDate = this.getEndDate();
    let timeTargetAnnotation = {
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
    };
    annotationsList.push(timeTargetAnnotation);

    if (this.goal != null) {
      let targetAmount = this.goal.targetAmount;
      let targetAmountAnnotation = {
        id: 'hline',
        type: 'line',
        mode: 'horizontal',
        scaleID: 'y-axis-0',
        value: targetAmount.toFixed(2),
        borderColor: '#002d78',
        borderWidth: 2,
        label: {
          backgroundColor: '#002d78',
          content: 'Target',// - ' + goalDate.format('MMM YYYY'),
          enabled: true,
          position: 'top'
        },
      };
      annotationsList.push(targetAmountAnnotation);
    }


    let annotation = {
      drawTime: 'afterDatasetsDraw',
      annotations: annotationsList
    }

    this.chartOptions.annotation = annotation;
  }

  private getStartDate() {
    return moment(this.portfolioPurchaseComponent.getStartDate()).startOf('date');
  }

  private getEndDate() {
    return moment(this.portfolioPurchaseComponent.getEndDate()).startOf('date');
  }

  private getStartingAmount() {
    return this.goal != null && this.goal.startingAmount > 0 ? this.goal.startingAmount : 100;
  }

  private getEstimatedDays() {

    let startDate = this.getStartDate();
    let endDate = this.getEndDate();

    return endDate.diff(startDate, 'days');
  }

  private getExtensionChartPoint() {
    return this.getEstimatedDays() + (this.getChartDaysInterval() * 5);
  }

  private buildChartLabels() {

    let startDate = this.getStartDate();
    let estimatedDays = this.getEstimatedDays();
    let extensionChartPoint = this.getExtensionChartPoint();

    let chartLabels = [];
    chartLabels.push(startDate.toDate());

    for (let day = 1; day <= extensionChartPoint; day++) {
      let date = startDate.add('day', 1).toDate();
      if (day % this.getChartDaysInterval() == 0 || day == estimatedDays) {
        chartLabels.push(date);
      }
    }

    this.chartLabels = chartLabels;
  }

  private buildChartData(monthPercent, label) {

    let chartData = [];
    if (monthPercent > 0) {
      let estimatedDays = this.getEstimatedDays();
      let extensionChartPoint = this.getExtensionChartPoint();
      chartData.push(this.getStartingAmount());

      let dailyPercent = this.calculateDailyPercent(monthPercent, 30);

      let beforeValues = [];
      beforeValues.push(this.getStartingAmount());

      for (let day = 1; day <= extensionChartPoint; day++) {

        let beforeValue = beforeValues[day - 1];
        let currencyValue = beforeValue + (beforeValue * dailyPercent / 100);

        if (this.goal != null && day % 30 == 0) {
          currencyValue += this.goal.monthlyContribution;
        }

        beforeValues.push(currencyValue)
        if (day % this.getChartDaysInterval() == 0 || day == estimatedDays) {
          chartData.push(currencyValue);
        }
      }
    }

    this.chartData.push({ data: chartData, label: label });
  }

  private getChartDaysInterval() {

    let estimatedDays = this.getEstimatedDays();

    if (estimatedDays >= 10800) { return 180; }
    if (estimatedDays >= 7200) { return 120; }
    if (estimatedDays >= 3600) { return 60; }
    if (estimatedDays >= 1800) { return 30; }
    if (estimatedDays >= 720) { return 15; }
    if (estimatedDays >= 60) { return 10; }
    else { return 1; }
  }

  private calculateDailyPercent(monthPercent, totalDays) {
    let aer = monthPercent * 1 / 100;
    let daily = 0;
    let monthly = 0;
    let tempaer = (1 + aer);
    daily = Math.pow(tempaer, (1 / totalDays)) - 1;
    daily = daily * 100;

    return daily;
  }
}
