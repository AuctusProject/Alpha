

import { Component, Injector } from '@angular/core';
import { BasePage } from './../base';
import { PortifolioService } from './../../services/portifolio.service';


@Component({
  selector: 'page-portifolio',
  templateUrl: 'portifolio.html',
})
export class PortifolioPage extends BasePage {

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

  constructor(public injector: Injector, private portifolioService: PortifolioService) {
    super(injector);
    this.getPortifolioDistribution();
  }

  private getPortifolioDistribution() {

    this.loadingHelper.showLoading();

    this.portifolioService.getDistribution()
      .subscribe(success.bind(this), error.bind(this));

    function success(response) {
      this.distribution = response[0].distribution;
      this.buildChart();
      this.loadingHelper.hideLoading();
    }

    function error(response) {
      this.loadingHelper.hideLoading();
    }
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
