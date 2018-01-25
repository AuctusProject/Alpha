import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'goal-option',
  templateUrl: './goal-option.component.html',
  styleUrls: ['./goal-option.component.css']
})
export class GoalOptionComponent implements OnInit {

  @Input() value: number;
  @Input() label: string;
  icon: string;

  constructor() { }

  ngOnInit() {
    if (this.label == 'Other'){
      this.icon = "assets/icons/other.png";
    }
    else if (this.label == 'Retirement'){
      this.icon = "assets/icons/retirement.png";
    }
    else if (this.label == 'College Savings'){
      this.icon = "assets/icons/college-savings.png";
    }
    else{
      this.icon = "assets/icons/house.png";
    }
  }

}
