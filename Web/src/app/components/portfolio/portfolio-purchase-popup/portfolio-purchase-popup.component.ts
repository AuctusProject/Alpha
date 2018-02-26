import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material"
import { Portfolio } from '../../../model/portfolio/portfolio';

@Component({
  selector: 'app-portfolio-purchase-popup',
  templateUrl: './portfolio-purchase-popup.component.html',
  styleUrls: ['./portfolio-purchase-popup.component.css']
})
export class PortfolioPurchasePopupComponent implements OnInit {

  portfolio: Portfolio;

  constructor(private dialogRef: MatDialogRef<PortfolioPurchasePopupComponent>,
    @Inject(MAT_DIALOG_DATA) data) {
      if (data && data.portfolio){
        this.portfolio = data.portfolio;
      }
  }

  ngOnInit() {
  }

}
