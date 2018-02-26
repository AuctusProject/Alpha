import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { MatDialog, MatDialogConfig } from "@angular/material";
import { PortfolioPurchasePopupComponent } from "../portfolio-purchase-popup/portfolio-purchase-popup.component";

@Component({
  selector: 'portfolio-allocation',
  templateUrl: './portfolio-allocation.component.html',
  styleUrls: ['./portfolio-allocation.component.css']
})
export class PortfolioAllocationComponent implements OnInit {

  @Input() portfolio: Portfolio;

  constructor(private dialog: MatDialog) { }

  ngOnInit() {

  }

  public onBuyClick() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      portfolio: this.portfolio
    };

    this.dialog.open(PortfolioPurchasePopupComponent, dialogConfig);
  }

}
