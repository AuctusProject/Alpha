import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/account/goal";
import { FullRegister } from "../../../model/account/fullRegister";
import { AccountService } from "../../../services/account.service";
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'register-step',
  templateUrl: './register-step.component.html',
  styleUrls: ['./register-step.component.css']
})
export class RegisterStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();
  fullRegisterDTO : FullRegister;
  promise : Subscription;


  constructor(private accountService : AccountService) { 
    this.fullRegisterDTO = new FullRegister();
  }

  ngOnInit() {
    
  }

  onSubmit(){
    // this.fullRegisterDTO.goal = this.model;
    // let onSubmitted = this.onSubmitted;
    // this.promise = this.accountService.fullRegister(this.fullRegisterDTO).subscribe(
    //   ret => {
    //     onSubmitted.emit(true);
    //   }
    // );
  }
}
