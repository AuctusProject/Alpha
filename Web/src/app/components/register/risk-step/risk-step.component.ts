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
      this.riskDescription = "1 Avoiding loss is the priority";
    }
    else if (this.model.risk == 2){

    }
    else if (this.model.risk == 3){
      
    }
    else if (this.model.risk == 4){

    }
    else if (this.model.risk == 5){
      this.riskDescription = "5 High risk"
    }
  }

  onSubmit(){
    let onSubmitted = this.onSubmitted;
    this.promise = this.accountService.setGoal(this.model).subscribe(
      ret => {
        this.notificationService.success("Success", "Goal saved");
        onSubmitted.emit(true);
      }
    );
  }

}
