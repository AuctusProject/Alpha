import { LoadingController } from 'ionic-angular';
import { Injectable } from '@angular/core';

@Injectable()
export class LoadingHelper {

    private loadingCount: number;

    private loading: any;

    constructor(private loadingCtrl: LoadingController) {
        this.loadingCount = 0;
    }

    public showLoading() {
        if (this.loadingCount == 0) {
            if (this.loading == null) {
                this.loading = this.loadingCtrl.create({
                    //spinner: 'hide',
                    //content: '<img src="assets/logo.gif" />',
                    //cssClass: 'my-loading-class'
                });
            }
            this.loading.present();
        }
        this.loadingCount++;
    }

    public hideLoading() {
        this.loadingCount--;
        if (this.loadingCount <= 0) {
            this.loadingCount = 0;
            this.loading.dismiss();
            this.loading = null;
        }
    }
}
