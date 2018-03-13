import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";
import { AccountService } from "../../../services/account.service";
import { GoalOption } from '../../../model/account/goalOption';
import { MatStepper } from "@angular/material";
import { WindowRefService } from "../../../services/window-ref.service";


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
  selected: boolean = false;
  width: string = '40%';
  cols: number = 4;

  options: GoalOption[];

  constructor(private accountService: AccountService, private winRef: WindowRefService) {
    this.accountService.listGoalOptions().subscribe(
      options => {
        this.options = options
      }
    )
  }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

  onOptionChange(optionId) {
    this.model.goalOption = this.options.filter(option => option.id == optionId)[0];
    this.selected = true;
  }

  onSubmit() {
    if (this.selected) {
      this.onSubmitted.emit(true);
    }
  }
}
