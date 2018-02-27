import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'percentage-label',
  templateUrl: './percentage-label.component.html',
  styleUrls: ['./percentage-label.component.css']
})
export class PercentageLabelComponent implements OnInit {
  @Input() percentage : number;
  constructor() { }

  ngOnInit() {
  }

  absolutPercentage(){
    return Math.abs(this.percentage);
  }
}
