
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

    public historicalChartOptions: any = {
        responsive: true,
        scales: {
            xAxes: [{
                display: true,
                gridLines: { drawOnChartArea: false },
                ticks: {
                    //fontFamily: 'HelveticaNeue', 
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
                    //fontFamily: 'HelveticaNeue',
                    callback: function (label, index, labels) {
                        return FormatHelper.formatToShortCurrency(label, '$');
                    }
                }
            }],
        },
        tooltips: {
            enabled: true,
            mode: 'single',
            callbacks: {
                title: function (tooltipItems, data) {
                    return moment(tooltipItems[0].xLabel).format('MMMM DD YYYY');
                },
                label: function (tooltipItems, data) {
                    let formatValue = FormatHelper.formatToShortCurrency(tooltipItems.yLabel, '$');
                    return formatValue;
                }
            }
        }
    }
    public lineChartLegend: boolean = true;
    public lineChartType: string = 'line';
    public historicalChartColors: Array<any> = [{
        backgroundColor: 'rgba(148,159,177,0.2)',
        borderColor: 'rgba(148,159,177,1)',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }];

    public historicalChartData: Array<any> = new Array<any>();
    public historicalChartLabels: Array<any> = new Array<any>();

    public history: any;

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.getPortfolioHistory()
    }

    private getPortfolioHistory() {
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

            var acum = 100;
            for (let i = 0; i < this.history.values.length; i++) {
                acum = acum * (1 + (this.history.values[i].value / 100.0));
                chartData.push(acum.toFixed(2));
                chartLabels.push(this.history.values[i].date);
            }

            this.historicalChartLabels = chartLabels;
            this.historicalChartData.push({ data: chartData });
        }
    }

}
