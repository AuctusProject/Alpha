import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material"
import { Portfolio } from '../../../model/portfolio/portfolio';
import { Goal } from '../../../model/account/goal';

@Component({
  selector: 'app-portfolio-purchase-popup',
  templateUrl: './portfolio-purchase-popup.component.html',
  styleUrls: ['./portfolio-purchase-popup.component.css']
})
export class PortfolioPurchasePopupComponent implements OnInit {

  portfolio: Portfolio;
  goal?: Goal;

  constructor(private dialogRef: MatDialogRef<PortfolioPurchasePopupComponent>,
    @Inject(MAT_DIALOG_DATA) data) {
      if (data){
        this.portfolio = data.portfolio;
        this.goal = data.goal;
      }
  }

  ngOnInit() {
  }

}
