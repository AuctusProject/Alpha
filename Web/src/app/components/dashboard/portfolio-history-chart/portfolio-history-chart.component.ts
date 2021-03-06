import { Component, OnInit, Input } from '@angular/core';
import { HistoryValues } from "../../../model/advisor/historyValues";

@Component({
  selector: 'portfolio-history-chart',
  templateUrl: './portfolio-history-chart.component.html',
  styleUrls: ['./portfolio-history-chart.component.css']
})
export class PortfolioHistoryChartComponent implements OnInit {
  @Input() historyValues: HistoryValues[];
  public historicalChartData: Array<any>;
  public historicalChartLabels: Array<any>;

  constructor() { }

  ngOnInit() {
    if (this.historyValues != undefined) {
      this.historicalChartData = [{ data: [] }];
      this.historicalChartLabels = [];
      var i = 0;
      var acum = 100;
      for (let value of this.historyValues) {
        acum = acum * (1 + (value.value / 100.0));
        i++;
        this.historicalChartData[0].data.push(acum.toFixed(2));
        this.historicalChartLabels.push(value.date);
      }
    }
  }

  public historicalChartOptions: any = {
    responsive: true,
    scales: {
      xAxes: [{
        display: true,
        gridLines: { drawOnChartArea: false },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, padding: 10
        },
        type: 'time',
        time: {
          unit: 'day',
          tooltipFormat: 'MMM D YYYY'
        }
      }],
      yAxes: [{
        gridLines: { borderDash: [3], borderDashOffset: [15], drawBorder: false, color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb'] },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, padding: 10, callback: function (value, index, values) {
            return '$' + value.toFixed(2);
          }
        }
      }]
    }
  };

  public lineChartLegend: boolean = true;
  public lineChartType: string = 'line';

  public historicalChartColorsOld: Array<any> = [
    {
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }];


  public historicalChartColors: Array<any> = [
    {
      backgroundColor: 'rgba(108, 168, 255,0.2)',
      borderColor: 'rgba(108, 168, 255,1)',
      pointBackgroundColor: 'rgba(108, 168, 255,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(108, 168, 255,0.8)'
    }];

}
