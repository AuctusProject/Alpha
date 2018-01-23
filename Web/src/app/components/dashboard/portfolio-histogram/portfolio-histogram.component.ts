import { Component, OnInit, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { HistogramInfo } from "../../../model/advisor/histogramInfo";

@Component({
  selector: 'portfolio-histogram',
  templateUrl: './portfolio-histogram.component.html',
  styleUrls: ['./portfolio-histogram.component.css']
})
export class PortfolioHistogramComponent implements OnInit {
  @Input() histogram: HistogramInfo[];
  historyDataSource: MatTableDataSource<HistogramInfo>;
  public histogramData: Array<any>;
  public histogramLabels: Array<any>;
  public histogramOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      xAxes: [{
        barPercentage: 1.0,
        categoryPercentage: 1.0 
      }]
    }
  };
  public showLegend:boolean = false;

  constructor() { }

  ngOnInit() {
    if (this.histogram != undefined) {
      this.histogramData = [{ data: [] }];
      this.histogramLabels = [];
      for (let histogramItem of this.histogram) {
        this.histogramData[0].data.push(histogramItem.quantity);
        this.histogramLabels.push((((histogramItem.lesser + histogramItem.greaterOrEqual)/2.0 - 100).toFixed(2)) + '%');
      }
    }

  }

}
