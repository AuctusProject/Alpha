import { Component, Input } from '@angular/core';
import { ModalController } from 'ionic-angular';


import { PurchaseSelectPage } from './../../pages/purchase-select/purchase-select';

@Component({
  selector: 'navbar',
  templateUrl: 'navbar.html'
})
export class NavbarComponent {

  @Input() public onPurchaseSelectClose: Function;

  constructor(public modalCtrl: ModalController) {
  }

  public openPurchaseSelect() {

    let params = {
    };

    let options = {
      enableBackdropDismiss: false
    };

    let purchaseSelect = this.modalCtrl.create(PurchaseSelectPage, params, options);

    purchaseSelect.onDidDismiss(selectedProjection => {
      if(this.onPurchaseSelectClose){
        this.onPurchaseSelectClose();
      }
    });

    purchaseSelect.present();
  }

}
