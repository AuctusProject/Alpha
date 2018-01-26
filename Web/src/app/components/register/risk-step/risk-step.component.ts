import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";
import {MediaChange, ObservableMedia} from "@angular/flex-layout";
import { Subscription } from 'rxjs/Subscription';
import { AccountService } from "../../../services/account.service";
import { NotificationsService } from "angular2-notifications";

@Component({
  selector: 'risk-step',
  templateUrl: './risk-step.component.html',
  styleUrls: ['./risk-step.component.css']
})
export class RiskStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();
  @Output() onValidationFail = new EventEmitter<number>();
  riskDescription: string;
  promise : Subscription;

  constructor(public media: ObservableMedia, 
      public accountService : AccountService,
      public notificationService : NotificationsService) { }

  ngOnInit() {
    this.setDescription();
  }

  onChangeSlider(){
    this.setDescription();
  }

  setDescription(){
    if (this.model.risk == 1){
      this.riskDescription = "1 - Conservative";
    }
    else if (this.model.risk == 2){
      this.riskDescription = "2 - Moderately Conservative";
    }
    else if (this.model.risk == 3){
      this.riskDescription = "3 - Moderately Aggressive";
    }
    else if (this.model.risk == 4){
      this.riskDescription = "4 - Aggressive";
    }
    else if (this.model.risk == 5){
      this.riskDescription = "5 - Very Aggressive"
    }
  }

  onSubmit(){
    let onSubmitted = this.onSubmitted;
    var goal : any = this.model;
    goal.goalOptionId = this.model.goalOption.id;
    if (!this.validFields()){
      return;
    }
    this.promise = this.accountService.setGoal(goal).subscribe(
      ret => {
        if (ret){
          this.notificationService.success("Success", "Goal saved");
          onSubmitted.emit(true);
        }
      }
    );
  }

  validFields() : boolean{
    return this.validGoalOption() && 
           this.validStartingAmount() &&
           this.validMonthlyContribution() &&
           this.validTimeframe() && 
           this.validRisk();
  }

  validGoalOption() : boolean{
    if (this.model.goalOption && this.model.goalOption.id){
      return true;
    }
    this.notificationService.error("Error", "Saving option must be filled.");
    this.onValidationFail.emit(0);
    return false;
  }

  validTimeframe() : boolean{
    if (this.model.timeframe){
      return true;
    }
    this.notificationService.error("Error", "Years must be more than 0 (zero).");
    this.onValidationFail.emit(2);
    return false;
  }

  validStartingAmount() : boolean{
    if (this.model.startingAmount || this.model.startingAmount == 0){
      return true;
    }
    this.notificationService.error("Error", "Starting amount must be filled.");
    this.onValidationFail.emit(1);
    return false;
  }

  validMonthlyContribution() : boolean{
    if (this.model.monthlyContribution || this.model.monthlyContribution == 0){
      return true;
    }
    this.notificationService.error("Error", "Monthly contribution must be filled.");
    this.onValidationFail.emit(1);
    return false;
  }

  validRisk() : boolean{
    if (this.model.risk){
      return true;
    }
    this.notificationService.error("Error", "Risk must be filled.");
    this.onValidationFail.emit(3);
    return false;
  }

}
