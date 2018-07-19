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
      this.historicalChartData = [{ data: [] }, { data: [] }];
      this.historicalChartLabels = [];
      var i = 0;
      var acum = 100;
      for (let value of this.historyValues) {
        acum = acum * (1 + (value.value / 100.0));
        var val = acum - 100.0;
        i++;
        if (val >= 0) {
          this.historicalChartData[0].data.push(val.toFixed(2));
          this.historicalChartData[1].data.push(null);
        }
        else {
          this.historicalChartData[0].data.push(null);
          this.historicalChartData[1].data.push(val.toFixed(2));
        }
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
          tooltipFormat: 'MM/DD/YYYY hh:mm'
        }
      }],
      yAxes: [{
        gridLines: { borderDash: [3], borderDashOffset: [15], drawBorder: false, color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb'] },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, padding: 10, callback: function (value, index, values) {
            return value.toFixed(1) + ' %';
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
    },
    {
      backgroundColor: 'rgba(245,81,95,0.2)',
      borderColor: 'rgba(245,81,95,1)',
      pointBackgroundColor: 'rgba(245,81,95,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(245,81,95,0.8)'
    }];
}
