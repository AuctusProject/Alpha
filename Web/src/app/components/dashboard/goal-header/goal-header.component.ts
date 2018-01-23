import { Component, Input, OnInit } from '@angular/core';
import { Goal } from "../../../model/account/goal";

@Component({
  selector: 'goal-header',
  templateUrl: './goal-header.component.html',
  styleUrls: ['./goal-header.component.css']
})
export class GoalHeaderComponent implements OnInit {
  @Input() model: Goal;

  constructor() {
    this.model = new Goal();
  }

  ngOnInit() {
    
  }

}
