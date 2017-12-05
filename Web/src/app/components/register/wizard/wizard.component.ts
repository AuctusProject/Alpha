import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'register-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {

  currentStep = 0;

  constructor() { }

  ngOnInit() {
    
  }

}
