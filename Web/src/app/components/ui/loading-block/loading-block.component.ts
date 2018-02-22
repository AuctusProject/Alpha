import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'loading-block',
  templateUrl: './loading-block.component.html',
  styleUrls: ['./loading-block.component.css']
})
export class LoadingBlockComponent implements OnInit {

  @Input() width: number;
  @Input() widthPercentage: number;
  @Input() height: number;
  
  constructor() { }

  ngOnInit() {
  }

}
