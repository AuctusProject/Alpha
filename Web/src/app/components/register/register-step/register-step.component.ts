import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from "../../../model/goal";
import { Login } from "../../../model/login";

@Component({
  selector: 'register-step',
  templateUrl: './register-step.component.html',
  styleUrls: ['./register-step.component.css']
})
export class RegisterStepComponent implements OnInit {

  @Input() model: Goal;
  @Output() modelChange = new EventEmitter<Goal>();
  @Output() onSubmitted = new EventEmitter<boolean>();
  login : Login;


  constructor() { }

  ngOnInit() {
  }

  onSubmit(){
    this.onSubmitted.emit(true);
  }
}
