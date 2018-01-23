import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";
import { AccountService } from "../../../services/account.service";
import { GoalOption } from '../../../model/goalOption';
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
  cols : number = 4;
  
  options: GoalOption[];

  constructor(private accountService: AccountService, private winRef: WindowRefService) { }

  ngOnInit() {
    this.accountService.listGoalOptions().subscribe(
      options => {
        this.options = options
      }
    )
    let windowWidth = this.winRef.nativeWindow.innerWidth;
    this.onWidth(innerWidth);
  }

  onOptionChange(optionId){
    this.model.goalOption = this.options.filter(option => option.id == optionId)[0];
    this.selected = true;
  }

  onSubmit(){
    if (this.selected){
      this.onSubmitted.emit(true);
    }
  }

  onWidth(width){
    if (width < 950) {
      this.cols = 3;
    }

    if (width > 950) {
      this.width = '40%';
      this.cols = 4;
    }

    if (width < 750) {
      this.width = '80%';
      this.cols = 2;
    }

    if (width < 550) {
      this.width = '100%';
      this.cols = 2;
    }

    if (width < 350) {
      this.width = '100%';
      this.cols = 1;
    }
  }

  onResize(event) {
    this.onWidth(event.target.innerWidth);
  }
}
