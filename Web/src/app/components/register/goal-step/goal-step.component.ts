import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";
import { AccountService } from "../../../services/account.service";
import { GoalOption } from '../../../model/goalOption';
import { MatStepper } from "@angular/material";

@Component({
  selector: 'goal-step',
  templateUrl: './goal-step.component.html',
  styleUrls: ['./goal-step.component.css']
})
export class GoalStepComponent implements OnInit {

  @Input() model: Goal;
  @Input() stepper: MatStepper;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();
  
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

  onSubmit(){
    this.onSubmitted.emit(true);
  }
}
