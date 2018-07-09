import { Component, OnInit, Input } from '@angular/core';
import { HistogramInfo } from "../../../model/advisor/histogramInfo";

@Component({
  selector: 'portfolio-histogram',
  templateUrl: './portfolio-histogram.component.html',
  styleUrls: ['./portfolio-histogram.component.css']
})
export class PortfolioHistogramComponent implements OnInit {
  @Input() histogram: HistogramInfo[];
  public histogramData: Array<any>;
  public histogramLabels: Array<any>;
  public histogramOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      xAxes: [{
        barPercentage: 1.0,
        categoryPercentage: 1.0,
        display: true,
        scaleLabel: {
          display: true,
          labelString: 'Daily performance'
        },
        gridLines: { drawOnChartArea: false },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, padding: 10
        },
        stacked:true
      }],
      yAxes: [{
        scaleLabel: {
          display: true,
          labelString: 'Days amount'
        },
        gridLines: { borderDash: [3], borderDashOffset: [15], drawBorder: false, color: ['#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb', '#bbbbbb'] },
        ticks: {
          fontFamily: 'HelveticaNeue', fontSize: 12, padding: 10
        },
        stacked:true
      }]
    }
  };
  public showLegend:boolean = false;

  constructor() { }

  ngOnInit() {
    if (this.histogram != undefined) {
      this.histogramData = [{ data: [] },{ data: [] }];
      this.histogramLabels = [];
      for (let histogramItem of this.histogram) {
        var labelValue = (histogramItem.lesser + histogramItem.greaterOrEqual) / 2.0;
        if(labelValue < 0){
          this.histogramData[0].data.push(histogramItem.quantity);
          this.histogramData[1].data.push(0);
        }
        else{
          this.histogramData[0].data.push(0);
          this.histogramData[1].data.push(histogramItem.quantity);
        }
        this.histogramLabels.push(labelValue.toFixed(2) + '%');
      }
    }

  }

  public histogramColors: Array<any> = [
    {
      backgroundColor: 'rgba(245,81,95,0.8)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    {
      backgroundColor: 'rgba(0,155,255,0.8)',
      borderColor: 'rgba(108, 168, 255,1)',
      pointBackgroundColor: 'rgba(108, 168, 255,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(108, 168, 255,0.8)'
    }];

}
