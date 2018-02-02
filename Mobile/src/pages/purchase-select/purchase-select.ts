import { ViewController } from 'ionic-angular';
import { Component, Injector } from '@angular/core';

import { BasePage } from './../base';
import { PortfolioService } from './../../services/portfolio.service';
import { StorageHelper } from '../../helpers/storage-helper';
import { LoadingHelper } from '../../helpers/loading-helper';

@Component({
  selector: 'page-purchase-select',
  templateUrl: 'purchase-select.html',
})
export class PurchaseSelectPage {

  public projection: any;
  public selectedPurchase: any;

  constructor(private storageHelper: StorageHelper,
    private loadingHelper: LoadingHelper,
    private portfolioService: PortfolioService,
    private viewCtrl: ViewController) {
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
