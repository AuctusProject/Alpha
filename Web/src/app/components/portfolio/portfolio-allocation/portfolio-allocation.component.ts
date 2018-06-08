import { Component, OnInit, Input, ChangeDetectorRef, ApplicationRef } from '@angular/core';
import { Portfolio } from '../../../model/portfolio/portfolio';
import { MatDialog, MatDialogConfig } from "@angular/material";
import { PortfolioPurchasePopupComponent } from "../portfolio-purchase-popup/portfolio-purchase-popup.component";
import { Goal } from '../../../model/account/goal';
import { AssetDistributionHistory } from '../../../model/asset/assetDistributionHistory';
import * as moment from 'moment';

@Component({
  selector: 'portfolio-allocation',
  templateUrl: './portfolio-allocation.component.html',
  styleUrls: ['./portfolio-allocation.component.css']
})
export class PortfolioAllocationComponent implements OnInit {
  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  currentAllocation: AssetDistributionHistory;

  constructor(private dialog: MatDialog, private ref: ChangeDetectorRef, private ref2: ApplicationRef) { }

  ngOnInit() {
    if(this.portfolio.assetDistributionHistory && this.portfolio.assetDistributionHistory.length > 0){
      this.currentAllocation = this.portfolio.assetDistributionHistory[0];
    }
  }

  public onBuyClick() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      portfolio: this.portfolio
    };
    if (this.goal){
      dialogConfig.data.goal = this.goal;
    }

    this.dialog.open(PortfolioPurchasePopupComponent, dialogConfig);
  }

  public formatDate(date){
    return moment(date).format('YYYY-MM-DD hh:mm:ss');
  }
}
