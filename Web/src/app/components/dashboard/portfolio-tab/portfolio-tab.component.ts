import { Component, OnInit, Input } from '@angular/core';
import { PortfolioService } from "../../../services/portfolio.service";
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";
import { MatTableDataSource } from '@angular/material';
import { AssetDistribution } from "../../../model/asset/assetDistribution";

@Component({
  selector: 'app-portfolio-tab',
  templateUrl: './portfolio-tab.component.html',
  styleUrls: ['./portfolio-tab.component.css']
})
export class PortfolioTabComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution[];
  portfolioDistribution: PortfolioDistribution;
  portfolioDataSource: MatTableDataSource<AssetDistribution>;
  colors = CHART_COLORS;
  
  constructor(private portfolioService: PortfolioService) {
    this.portfolioDistributionModel = [];
  }

  ngOnInit() {
    this.portfolioService.getPortfoliosDistribution().subscribe(
      portfoliosDistribution => {
        if (portfoliosDistribution != undefined) {
          this.portfolioDistributionModel = portfoliosDistribution;
          this.portfolioDistribution = portfoliosDistribution[0];
          this.fillChartInformation();
        }
      }
    )
  }

  private fillChartInformation() {
    this.pieChartLabels.length = 0;
    this.pieChartData = [];
    for (let assetDistribution of this.portfolioDistribution.distribution) {
      this.pieChartLabels.push(assetDistribution.code);
      this.pieChartData.push(assetDistribution.percentage);
      if (assetDistribution.type == 1){
        this.totalCryptoPercentage += assetDistribution.percentage;
      }
      else {
        this.totalTraditionalPercentage += assetDistribution.percentage;
      }
    }
    this.portfolioDataSource = new MatTableDataSource<AssetDistribution>();
    this.portfolioDataSource.data = this.portfolioDistribution.distribution;
  }

  public totalTraditionalPercentage = 0;
  public totalCryptoPercentage = 0;

  

  public pieChartLabels: string[] = [];
  public pieChartData: number[] = [];
    public pieColors = [{ // grey
      backgroundColor: CHART_COLORS,
      borderWidth: [0,0,0]
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
