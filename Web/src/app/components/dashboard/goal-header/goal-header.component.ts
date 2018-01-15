import { Component, Input, OnInit } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { Goal } from "../../../model/goal";

@Component({
  selector: 'goal-header',
  templateUrl: './goal-header.component.html',
  styleUrls: ['./goal-header.component.css']
})
export class GoalHeaderComponent implements OnInit {
  @Input() model: Goal;

  constructor(private adviceService: AdviceService) {
    this.model = new Goal();
  }

  ngOnInit() {
    
  }

}
