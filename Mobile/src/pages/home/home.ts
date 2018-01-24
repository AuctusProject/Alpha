import { StorageHelper } from './../../helpers/storage-helper';
import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { LoginPage } from '../login/login';
import { TabsPage } from '../tabs/tabs';



@Component({
    selector: 'page-home',
    templateUrl: 'home.html'
})

export class HomePage {

    public pushPage: any;

    constructor(public navCtrl: NavController,
        public storageHelper: StorageHelper) {
        this.pushPage = LoginPage
    }

    ionViewDidEnter() {
        if (this.storageHelper.getLoginToken() !== 'undefined') {
            this.navCtrl.push(TabsPage);
        }
    }

}
