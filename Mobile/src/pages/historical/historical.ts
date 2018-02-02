
import { Component, Injector } from '@angular/core';

import { BasePage } from './../base';
import { FormatHelper } from './../../helpers/format-helper';
import { PortfolioService } from './../../services/portfolio.service';

import moment from 'moment';

@Component({
    selector: 'page-historical',
    templateUrl: 'historical.html',
})
export class HistoricalPage extends BasePage {

    public chartColors: Array<any> = [{
        backgroundColor: 'rgba(148,159,177,0.2)',
        borderColor: 'rgba(148,159,177,1)',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }];
    public chartData: Array<any> = new Array<any>();
    public chartLabels: Array<string> = new Array<string>();
    public chartLegend: boolean = true;
    public chartOptions: any = {
        responsive: true,
        scales: {
            xAxes: [{
                display: true,
                gridLines: { drawOnChartArea: false },
                ticks: {
                    fontFamily: 'HelveticaNeueMedium',
                },
                type: 'time',
                time: { tooltipFormat: 'MMM D YYYY' }
            }],
            yAxes: [{
                gridLines: {
                    borderDash: [3],
                    borderDashOffset: [15],
                    drawBorder: false,
                    color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb']
                },
                ticks: {
                    fontFamily: 'HelveticaNeueMedium',
                    callback: function (label, index, labels) {
                        return FormatHelper.formatCurrency(label, '$');
                    }
                }
            }],
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
                    let formatValue = FormatHelper.formatCurrency(tooltipItems.yLabel, '$');
                    return formatValue;
                }
            }
        }
    }
    public chartType: string = 'line';

    public history: any;
    public filterSelected: string;

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.onPurchaseSelectClose = this.buildHistorical.bind(this);
        this.filterSelected = 'days';
    }

    ionViewWillEnter() {
        if (this.storageHelper.getSelectedPurchase()) {
            this.buildHistorical();
        } else {
            this.openPurchaseSelect();
        }
    }

    public buildHistorical() {
        if (this.selectedPurchase != this.storageHelper.getSelectedPurchase()) {
            this.selectedPurchase = this.storageHelper.getSelectedPurchase();
            this.getHistorical();
        }
    }

    private getHistorical() {
        this.loadingHelper.showLoading();
        this.portfolioService.getHistory().subscribe(
            success => {
                this.history = success[0];
                this.buildChart();
                this.loadingHelper.hideLoading();
            }, error => {
                this.loadingHelper.hideLoading();
            }
        );
    }

    private buildChart() {
        if (this.history != undefined) {

            let chartData = [];
            let chartLabels = [];
            let acum = 100;
            let filterConfig = this.getFilterConfig();

            let historyValuesList = this.lodash.filter(this.history.values, function (item) {
                return moment(item.date) >= filterConfig.date;
            });

            for (let index = 0; index < historyValuesList.length; index++) {
                acum = acum * (1 + (historyValuesList[index].value / 100.0));

                if (index % filterConfig.pointIndex == 0) {
                    chartData.push(acum.toFixed(2));
                    chartLabels.push(this.history.values[index].date);
                }
            }

            this.chartLabels = chartLabels;
            this.chartData[0] = { data: chartData };
        }
    }

    public filterChanged(event) {
        this.buildChart();
    }

    private getFilterConfig(): any {
        switch (this.filterSelected) {
            case 'days': {
                return {
                    date: this.moment().add('days', -30),
                    pointIndex: 1
                }
            }
            case 'months': {
                return {
                    date: this.moment().add('month', -6),
                    pointIndex: 6
                }
            }
            case 'all': {
                let totalDays = this.moment().diff(this.moment(this.history.values[0].date), 'days');

                return {
                    date: this.moment(this.history.values[0].date),
                    pointIndex: totalDays < 30 ? 1 : totalDays < 360 ? 6 : 15
                }
            }
        }

    }
}
