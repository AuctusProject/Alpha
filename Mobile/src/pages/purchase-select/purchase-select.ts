import { Component, Injector } from '@angular/core';

import { BasePage } from './../base';
import { PortfolioService } from './../../services/portfolio.service';

@Component({
  selector: 'page-purchase-select',
  templateUrl: 'purchase-select.html',
})
export class PurchaseSelectPage extends BasePage {

  public projection: any;
  public selectedPurchase: any;

  constructor(public injector: Injector, private portfolioService: PortfolioService) {
    super(injector);
    this.getProjection();
    this.selectedPurchase = this.storageHelper.getSelectedPurchase();
  }

  private getProjection() {

    this.loadingHelper.showLoading();

    this.portfolioService.getProjection()
      .subscribe(success => {
        this.projection = success;
        this.loadingHelper.hideLoading();
      }, error => {
        this.loadingHelper.hideLoading();
      });
  }

  public onPurchaseClick() {
    this.storageHelper.setSelectedPurchase(this.selectedPurchase);
  }

  public closeModal() {
    this.viewCtrl.dismiss(this.selectedPurchase);
  }

}
