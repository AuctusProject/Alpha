import { Component, OnInit, Input } from '@angular/core';
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";
import { MatTableDataSource } from '@angular/material';
import { AssetDistribution } from "../../../model/asset/assetDistribution";

@Component({
  selector: 'portfolio-distribution',
  templateUrl: './portfolio-distribution.component.html',
  styleUrls: ['./portfolio-distribution.component.css']
})
export class PortfolioDistributionComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution;
  portfolioDataSource: MatTableDataSource<AssetDistribution>;
  colors = CHART_COLORS;

  constructor() {
  }

  ngOnInit() {
    if (this.portfolioDistributionModel != undefined) {
      this.pieChartLabels.length = 0;
      this.pieChartData = [];
      for (let assetDistribution of this.portfolioDistributionModel.distribution) {
        this.pieChartLabels.push(assetDistribution.code);
        this.pieChartData.push(assetDistribution.percentage);
        if (assetDistribution.type == 1) {
          this.totalCryptoPercentage += assetDistribution.percentage;
        }
        else {
          this.totalTraditionalPercentage += assetDistribution.percentage;
        }
      }
      this.portfolioDataSource = new MatTableDataSource<AssetDistribution>();
      this.portfolioDataSource.data = this.portfolioDistributionModel.distribution;
    }
  }

  public totalTraditionalPercentage = 0;
  public totalCryptoPercentage = 0;



  public pieChartLabels: string[] = [];
  public pieChartData: number[] = [];
  public pieColors = [{ // grey
    backgroundColor: CHART_COLORS,
    borderWidth: [0, 0, 0]
  }
  ];

  public pieChartOptions: any = [{
    layout: {
      padding: {
        left: 50,
        right: 0,
        top: 100,
        bottom: 0
      }
    }
  }];
  public pieChartBorder: number[] = [0, 0, 0];

  public pieChartType: string = 'pie';

  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

}

const CHART_COLORS: string[] = ['#4bebee', '#14bdc0', '#9a43e7', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6'];
