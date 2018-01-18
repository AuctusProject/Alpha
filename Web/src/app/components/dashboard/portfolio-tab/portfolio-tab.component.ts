import { Component, OnInit, Input } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { PortfolioDistribution } from "../../../model/advice/portfolioDistribution";

@Component({
  selector: 'app-portfolio-tab',
  templateUrl: './portfolio-tab.component.html',
  styleUrls: ['./portfolio-tab.component.css']
})
export class PortfolioTabComponent implements OnInit {
  @Input() portfolioDistributionModel: PortfolioDistribution[];
  portfolioDistribution: PortfolioDistribution;
  
  constructor(private adviceService: AdviceService) {
    this.portfolioDistributionModel = [];
  }

  ngOnInit() {
    this.adviceService.getPortfoliosDistribution().subscribe(
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
      this.pieChartLabels.push(assetDistribution.name);
      this.pieChartData.push(assetDistribution.percentage);
    }
  }

    public pieChartLabels: string[] = [];
    public pieChartData: number[] = [];
    public pieChartOptions: any = {
      borderWidth: [0,0,0]
    };
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
