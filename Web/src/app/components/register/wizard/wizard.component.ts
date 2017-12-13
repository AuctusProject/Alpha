import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'register-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {
  isLinear = true;
  periodStep: FormGroup;

  constructor(private _formBuilder: FormBuilder) { }

  ngOnInit() {
    this.goalStepForm = this._formBuilder.group({
      goal: '',
      targetAmount: ''
    });
    this.periodStep = this._formBuilder.group({
      periodCtrl: ''
    });
  }

}
