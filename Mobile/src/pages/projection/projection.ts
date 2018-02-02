import { Purchase } from './../../models/purchase.model';
import { Component, Injector } from '@angular/core';

import { BasePage } from './../base';
import { FormatHelper } from '../../helpers/format-helper';
import { PortfolioService } from './../../services/portfolio.service';
import { Projection } from './../../models/projection.model';

import Chart from 'chart.js';
import 'chartjs-plugin-annotation';
import moment from 'moment';


@Component({
    selector: 'page-projection',
    templateUrl: 'projection.html',
})

export class ProjectionPage extends BasePage {

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
                        return FormatHelper.formatCurrency(label, '$');
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
                    let formatValue = FormatHelper.formatCurrency(tooltipItems.yLabel, '$');
                    return data.datasets[tooltipItems.datasetIndex].label + ":" + formatValue;
                }
            }
        },
        annotation: null
    };
    public chartType: string = 'line';

    public projection: Projection;
    public purchase: Purchase;

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.onPurchaseSelectClose = this.buildProjection.bind(this);
    }

    ionViewWillEnter() {
        if (this.storageHelper.getSelectedPurchase()) {
            this.buildProjection();
        } else {
            this.openPurchaseSelect();
        }
    }

    public buildProjection() {
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
                this.purchase = this.lodash.find(this.projection.purchases, { 'advisorId': this.selectedPurchase })
                this.buildChart();
                this.loadingHelper.hideLoading();
            },
            error => {
                this.loadingHelper.hideLoading();
            });
    }

    private buildChart() {
        this.buildChartLabels();
        this.buildMonthyContributionChart();
        this.buildChartData(this.purchase.projectionData.pessimisticPercent, 'Pessimistic');
        this.buildChartData(this.purchase.projectionData.projectionPercent, 'Projection');
        this.buildChartData(this.purchase.projectionData.optimisticPercent, 'Optimistic');
        this.buildTargetLine();
    }


    private buildTargetLine() {

        let goalDate = this.getGoalDate();

        let annotation = {
            drawTime: 'afterDatasetsDraw',
            annotations: [{
                id: 'vline',
                type: 'line',
                mode: 'vertical',
                scaleID: 'x-axis-0',
                value: goalDate.format('YYYY-MM-DD'),
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

    private getPurchaseDate() {
        return this.moment(this.purchase.purchaseDate);
    }

    private getGoalDate() {

        let purchaseDate = this.getPurchaseDate();
        let goalDate = this.moment(purchaseDate)
            .add('years', this.projection.currentGoal.timeframe);

        return goalDate;
    }

    private getEstimatedMonths() {

        let purchaseDate = this.getPurchaseDate();
        let goalDate = this.getGoalDate();

        return goalDate.diff(purchaseDate, 'months');
    }

    private getExtensionChartPoint() {
        return Math.round(this.getEstimatedMonths() * 1.2);
    }

    private buildChartLabels() {

        let purchaseDate = this.getPurchaseDate();
        let estimatedMonths = this.getEstimatedMonths();
        let extensionChartPoint = this.getExtensionChartPoint();

        let chartLabels = [];
        chartLabels.push(purchaseDate.toDate());

        for (let month = 1; month <= extensionChartPoint; month++) {

            let date = purchaseDate.add('month', 1).toDate();

            if ((month <= estimatedMonths && (month % this.projection.currentGoal.timeframe == 0 || month == estimatedMonths)) || month == extensionChartPoint) {
                chartLabels.push(date);
            }
        }

        this.chartLabels = chartLabels;
    }

    private buildChartData(monthPercent, label) {

        let estimatedMonths = this.getEstimatedMonths();
        let extensionChartPoint = this.getExtensionChartPoint();

        var projectionValue = [];
        projectionValue.push(this.projection.currentGoal.startingAmount);

        let chartData = [];
        chartData.push(this.projection.currentGoal.startingAmount);

        for (let month = 1; month <= extensionChartPoint; month++) {

            let beforeValue = projectionValue[month - 1];
            let currencyValue = beforeValue + (beforeValue * monthPercent / 100);

            currencyValue += this.projection.currentGoal.monthlyContribution;
            projectionValue.push(currencyValue);

            if (month % this.projection.currentGoal.timeframe == 0 || month == estimatedMonths) {
                chartData.push(currencyValue);
            }
        }

        this.chartData.push({ data: chartData, label: label })
    }

    private buildMonthyContributionChart() {

        let estimatedMonths = this.getEstimatedMonths();

        var projectionValue = [];
        projectionValue.push(0);

        let chartData = [];
        chartData.push(0);

        let extensionChartPoint = this.getExtensionChartPoint();

        for (let month = 1; month <= extensionChartPoint; month++) {

            let beforeValue = projectionValue[month - 1];
            let currencyValue = beforeValue + this.projection.currentGoal.monthlyContribution;
            projectionValue.push(currencyValue);

            if (month % this.projection.currentGoal.timeframe == 0 || month == estimatedMonths) {
                chartData.push(currencyValue);
            }
        }

        this.chartData.push({ data: chartData, label: 'Contributed' })
    }

}
