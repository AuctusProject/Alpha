import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { PortfolioDistribution } from "../../../model/portfolio/portfolioDistribution";
import { MatTableDataSource } from '@angular/material';
import { AssetDistribution } from "../../../model/asset/assetDistribution";

@Component({
  selector: 'portfolio-distribution',
  templateUrl: './portfolio-distribution.component.html',
  styleUrls: ['./portfolio-distribution.component.css']
})
export class PortfolioDistributionComponent implements OnInit, OnChanges {
  @Input() assetDistributions: AssetDistribution[];
  portfolioDataSource: MatTableDataSource<AssetDistribution>;
  colors = CHART_COLORS;


  constructor() {
  }

  ngOnInit() {
    this.fillChartInformation();
  }

  ngOnChanges(changes: SimpleChanges){
   this.fillChartInformation(); 
  }

  fillChartInformation()
  {
    if (this.assetDistributions != undefined) {
      this.pieChartLabels.length = 0;
      this.pieChartData = [];
      for (let assetDistribution of this.assetDistributions) {
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
      this.portfolioDataSource.data = this.assetDistributions;
    }
  }

  showCurrentPercentage() {
    return this.portfolioDataSource.data.length > 0 && !!this.portfolioDataSource.data[0].currentPercentage;
  }

  getHeaderRowDef() {
    return this.showCurrentPercentage() ? ['color', 'holdings', 'percentage', 'currentpercentage']
      : ['color', 'holdings', 'percentage'];
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

const CHART_COLORS: string[] = [
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
'#14bdc0', '#9a43e7', '#4bebee', '#9013fe', '#5a44ba', '#4e2aa2', '#0243b7', '#3c91e6',
];
