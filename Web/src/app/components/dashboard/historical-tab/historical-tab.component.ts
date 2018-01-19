import { Component, OnInit, Input } from '@angular/core';
import { PortfolioHistory } from "../../../model/advice/portfolioHistory";
import { HistoryValues } from "../../../model/advice/historyValues";
import { AdviceService } from "../../../services/advice.service";
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-historical-tab',
  templateUrl: './historical-tab.component.html',
  styleUrls: ['./historical-tab.component.css']
})
export class HistoricalTabComponent implements OnInit {
  @Input() portfoliosHistoryModel: PortfolioHistory[];
  historyDataSource: MatTableDataSource<HistoryValues>;
  public historicalChartData: Array<any>;
  public historicalChartLabels: Array<any>;

  constructor(private adviceService: AdviceService) { }

  ngOnInit() {
    this.adviceService.getPortfoliosHistory().subscribe(
      portfoliosHistory => {
        if (portfoliosHistory != undefined) {
          this.portfoliosHistoryModel = portfoliosHistory;
          this.historicalChartData = [{ data: [] }];
          this.historicalChartLabels = [];
          var i = 0;
          var acum = 0;
          for (let value of portfoliosHistory[0].values) {
            if (i == 0) {
              acum = value.value;
            }
            else {
              acum = (acum * value.value / 100.0);
            }
            i++;
            this.historicalChartData[0].data.push(acum);
            this.historicalChartLabels.push(value.date);
          }
        }
      }
    );
  }

  public historicalChartOptions: any = {
    responsive: true,
    scales: {
      xAxes: [{
        display: true, 
        gridLines: { drawOnChartArea: false },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, fontColor: '9b9b9b', padding: 20
        },
        type: 'time'
      }],
      yAxes: [{
        gridLines: { borderDash: [3], borderDashOffset: [15], drawBorder: false, color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb'] },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, fontColor: '9b9b9b', padding: 10}
      }]
  }
};

  public lineChartLegend: boolean = true;
  public lineChartType: string = 'line';

  public historicalChartColors: Array < any > = [
  { // grey
    backgroundColor: 'rgba(148,159,177,0.2)',
    borderColor: 'rgba(148,159,177,1)',
    pointBackgroundColor: 'rgba(148,159,177,1)',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(148,159,177,0.8)'
  }];

}
