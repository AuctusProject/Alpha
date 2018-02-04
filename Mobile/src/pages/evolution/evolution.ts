import { PortfolioService } from './../../services/portfolio.service';

import { Component, Injector } from '@angular/core';
import { BasePage } from './../base';
import { FormatHelper } from './../../helpers/format-helper'

import Chart from 'chart.js';
import 'chartjs-plugin-annotation';
import moment from 'moment';

@Component({
    selector: 'page-evolution',
    templateUrl: 'evolution.html',
})

export class EvolutionPage extends BasePage {

    public chartColors: Array<any> = [
        {
            //Projection
            backgroundColor: 'rgb(74, 144, 226)',
            borderColor: 'rgb(74, 144, 226)',
            fill: 'false'
        },
        {
            //Historical
            backgroundColor: "rgb(15, 215, 185)",
            borderColor: "rgb(15, 215, 185)",
            fill: 'false'
        }
    ];
    public chartData: Array<any> = new Array<any>();
    public chartLabels: Array<string> = new Array<string>();
    public chartLegend: boolean = true;
    public chartOptions: any = {
        responsive: true,
        scales: {
            xAxes: [{
                drawBorder: false,
                drawOnChartArea: false,
                display: true,
                type: 'time',
                ticks: {
                    fontFamily: 'HelveticaNeueMedium',
                },
            }],
            yAxes: [{
                display: false
            }]
        },
        legend: {
            display: false
        },
        tooltips: {
            enabled: true,
            callbacks: {
                title: function (tooltipItems, data) {
                    return moment(tooltipItems[0].xLabel).format('MMMM DD YYYY');
                },
                label: function (tooltipItems, data) {
                    let formatValue = FormatHelper.formatCurrency(tooltipItems.yLabel, '$');
                    return formatValue;
                }
            }
        },
        annotation: null
    };
    public chartType: string = 'line';

    private history: any;
    private projection: any;
    private purchase: any;

    private summary = {
        today: {
            historical: null,
            projection: null,
            lossProfit: null
        }
    }

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.onPurchaseSelectClose = this.buildEvolution.bind(this);
    }

    ionViewWillEnter() {
        if (this.storageHelper.getSelectedPurchase()) {
            this.buildEvolution();
        } else {
            this.openPurchaseSelect();
        }
    }

    public buildEvolution() {
        if (this.selectedPurchase != this.storageHelper.getSelectedPurchase()) {
            this.selectedPurchase = this.storageHelper.getSelectedPurchase();
            this.getProjection();
        }
    }

    private getProjection() {
        this.loadingHelper.showLoading();
        this.portfolioService.getProjection().subscribe(
            success => {
                this.projection = success;
                this.setSelectedPurchase();
                this.buildChartLabels();
                this.buildProjectionChart();
                this.getHistory();
                this.buildTodayLine();
                this.loadingHelper.hideLoading();
            },
            error => {
                this.loadingHelper.hideLoading();
            });
    }

    private getHistory() {
        this.loadingHelper.showLoading();
        this.portfolioService.getHistory().subscribe(
            success => {
                this.history = success[0];
                this.buildHistoryChart();
                this.loadingHelper.hideLoading();
            }, error => {
                this.loadingHelper.hideLoading();
            }
        );
    }

    public setSelectedPurchase() {
        this.purchase = this.lodash.find(this.projection.purchases, { 'advisorId': this.selectedPurchase })
    }

    private buildProjectionChart() {

        let chartData = [];
        chartData.push(this.projection.currentGoal.startingAmount);

        let monthPercent = this.purchase.projectionData.projectionPercent;

        let estimatedTime = this.getEstimatedTime();
        let projectionDate = this.getPurchaseDate();
        let nextMonth = this.getPurchaseDate().add('months', 1);
        let monthDays = nextMonth.diff(projectionDate, 'days');

        let dailyPercent = this.calculateDailyPercent(monthPercent, monthDays);

        for (let time = 1; time <= estimatedTime; time++) {

            let beforeValue = chartData[time - 1];
            let currencyValue = beforeValue + (beforeValue * dailyPercent / 100);
            projectionDate.add('days', 1);
            if (projectionDate.format('YYYY-MM-DD') == nextMonth.format('YYYY-MM-DD')) {
                nextMonth.add('months', 1);
                monthDays = nextMonth.diff(projectionDate, 'days');
                dailyPercent = this.calculateDailyPercent(monthPercent, monthDays);
                currencyValue += this.projection.currentGoal.monthlyContribution;
            }
            chartData.push(currencyValue);

            if (projectionDate.format('YYYY-MM-DD') == this.moment().format('YYYY-MM-DD')) {
                this.summary.today.projection = currencyValue;
                this.calculateSummaryLossProfit();
            }
        }

        this.chartData.push({ data: chartData })
    }

    public getPurchaseDate() {
        return this.moment(this.purchase.purchaseDate);
    }

    private getEstimatedTime() {
        let purchaseDate = this.getPurchaseDate();
        let currentDate = this.moment();

        return currentDate.diff(purchaseDate, 'days') + 15;
    }

    private buildChartLabels() {

        let purchaseDate = this.getPurchaseDate();
        let estimatedTime = this.getEstimatedTime();

        let chartLabels = [];
        chartLabels.push(purchaseDate.toDate());

        for (let time = 1; time <= estimatedTime; time++) {

            let date = purchaseDate.add('day', 1).toDate();
            chartLabels.push(date);
        }

        this.chartLabels = chartLabels;
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

    private buildHistoryChart() {
        let chartData = [];
        let chartLabels = [];
        var acum = this.projection.currentGoal.startingAmount;

        let projectionDate = this.getPurchaseDate();
        let nextMonth = this.getPurchaseDate().add('months', 1);

        let historyList = this.lodash.filter(this.history.values, function (item) {
            return moment(item.date) >= projectionDate;
        });

        for (let time = 0; time < historyList.length; time++) {
            acum = acum * (1 + (historyList[time].value / 100.0));
            if (projectionDate.format('YYYY-MM-DD') == nextMonth.format('YYYY-MM-DD')) {
                nextMonth.add('months', 1);
                acum += this.projection.currentGoal.monthlyContribution;
            }
            chartData.push(acum.toFixed(2));

            if (projectionDate.format('YYYY-MM-DD') == this.moment().format('YYYY-MM-DD')) {
                this.summary.today.historical = acum;
                this.calculateSummaryLossProfit();
            }
            projectionDate.add('days', 1);
        }
        this.chartData.push({ data: chartData });
    }

    private calculateSummaryLossProfit() {
        if (this.summary.today.projection && this.summary.today.historical) {
            let lossProfit = (this.summary.today.historical / this.summary.today.projection - 1) * 100;
            console.log(lossProfit);
            this.summary.today.lossProfit = lossProfit.toFixed(5);
        }
    }

    private buildTodayLine() {

        let todayDate = this.moment();

        let annotation = {
            drawTime: 'afterDatasetsDraw',
            annotations: [{
                id: 'vline',
                type: 'line',
                mode: 'vertical',
                scaleID: 'x-axis-0',
                value: todayDate.format('YYYY-MM-DD'),
                borderColor: '#002d78',
                borderWidth: 2,
                label: {
                    backgroundColor: '#002d78',
                    content: 'Today',
                    enabled: true,
                    position: 'top'
                },
            }]
        }

        this.chartOptions.annotation = annotation;
    }

}
