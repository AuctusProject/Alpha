import { Component, OnInit, Input } from '@angular/core';
import { Goal } from "../../../model/goal";
import { AccountService } from "../../../services/account.service";
import { GoalOption } from '../../../model/goalOption';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { ControlValueAccessor } from '@angular/forms';

@Component({
  selector: 'goal-step',
  templateUrl: './goal-step.component.html',
  styleUrls: ['./goal-step.component.css']
})
export class GoalStepComponent implements OnInit, ControlValueAccessor {

  @Input() model: Goal;
  options: GoalOption[];

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.accountService.listGoalOptions().subscribe(
      options => {
        this.options = options
      }
    )
  }

  writeValue(value: any){
  }

  registerOnChange(fn){
  }

  registerOnTouched(){
  }
}
