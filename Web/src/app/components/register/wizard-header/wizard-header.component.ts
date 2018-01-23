import { Component, OnInit } from '@angular/core';
import {MediaChange, ObservableMedia} from "@angular/flex-layout";

@Component({
  selector: 'wizard-header',
  templateUrl: './wizard-header.component.html',
  styleUrls: ['./wizard-header.component.css']
})
export class WizardHeaderComponent implements OnInit {

  constructor(public media: ObservableMedia) { }

  ngOnInit() {
  }

}
