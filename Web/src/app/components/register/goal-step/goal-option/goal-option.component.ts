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
    if (this.label == 'Child'){
      this.icon = "assets/icons/child.png";
    }
    else {
      this.icon = "assets/icons/child.png";
    }
  }

}
