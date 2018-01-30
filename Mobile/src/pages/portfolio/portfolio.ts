

import { Component, Injector } from '@angular/core';

import { BasePage } from './../base';
import { PortfolioService } from './../../services/portfolio.service';

@Component({
    selector: 'page-portfolio',
    templateUrl: 'portfolio.html',
})
export class PortfolioPage extends BasePage {

    public assetColors: Array<string> = ['#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6'];

    public chartBorder: Array<number> = [0, 0, 0];
    public chartColors: Array<any> = [{ backgroundColor: this.assetColors, borderWidth: [0, 0, 0] }];
    public chartData: Array<number> = new Array<number>();
    public chartLabels: Array<string> = new Array<string>();
    public chartLegend: boolean = true;
    public chartOptions: any = {
        responsive: true, 
        legend: {
            display: false
        }
    };
    public chartType: string = 'pie';

    public distribution: any;
    public totalTraditionalPercentage: number = 0;
    public totalCryptoPercentage: number = 0;

    public onPurchaseSelectClose: Function;

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.getPortfolioDistribution();
        this.onPurchaseSelectClose = this.updateDistribution.bind(this);
    }

    public updateDistribution() {
        console.log("Distribution:" + this.storageHelper.getSelectedPurchase());
    }

    private getPortfolioDistribution() {

        this.loadingHelper.showLoading();

        this.portfolioService.getDistribution()
            .subscribe(success => {
                this.distribution = success[0].distribution;
                this.buildChart();
                this.loadingHelper.hideLoading();
            }, error => {
                this.loadingHelper.hideLoading();
            });
    }

    private buildChart() {
        if (this.distribution != undefined) {

            this.chartLabels.length = 0;
            this.chartData = [];

            for (let asset of this.distribution) {
                this.chartLabels.push(asset.code);
                this.chartData.push(asset.percentage);
                if (asset.type == 1) {
                    this.totalCryptoPercentage += asset.percentage;
                } else {
                    this.totalTraditionalPercentage += asset.percentage;
                }
            }

        }
    }
}
