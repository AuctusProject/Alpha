

import { Component, Injector } from '@angular/core';
import { BasePage } from './../base';
import { PortfolioService } from './../../services/portfolio.service';


@Component({
    selector: 'page-portfolio',
    templateUrl: 'portfolio.html',
})
export class PortfolioPage extends BasePage {

    public assetColors = ['#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6'];

    public distribution: any;
    public totalTraditionalPercentage = 0;
    public totalCryptoPercentage = 0;

    public chartLabels: string[] = [];
    public chartData: number[] = [];
    public chartOptions: any = [{ layout: { padding: { left: 50, right: 0, top: 100, bottom: 0 } } }];
    public chartBorder: number[] = [0, 0, 0];
    public chartType: string = 'pie';
    public chartColors = [{ backgroundColor: this.assetColors, borderWidth: [0, 0, 0] }];

    constructor(public injector: Injector, private portfolioService: PortfolioService) {
        super(injector);
        this.getPortfolioDistribution();
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
