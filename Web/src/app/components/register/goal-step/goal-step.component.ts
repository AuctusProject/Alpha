import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";
import { AccountService } from "../../../services/account.service";
import { GoalOption } from '../../../model/goalOption';

@Component({
  selector: 'goal-step',
  templateUrl: './goal-step.component.html',
  styleUrls: ['./goal-step.component.css']
})
export class GoalStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  
  options: GoalOption[];

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.accountService.listGoalOptions().subscribe(
      options => {
        this.options = options
      }
    )
  }

  onOptionChange(event){
    this.model.GoalOption = this.options.filter(option => option.id == event.value)[0];
  }
}
