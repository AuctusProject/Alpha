import { Injectable } from '@angular/core';
import { AlertController } from 'ionic-angular';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class AlertHelper {

    private cancelButtonLabel: string;
    private noButtonLabel: string;
    private okButtonLabel: string;
    private yesButtonLabel: string;

    private errorAlertTitle: string;
    private okAlertTitle: string;
    private yesNoAlertTitle: string;

    constructor(private alertController: AlertController, private translateService: TranslateService) {
        this.setTranslateFields();
    }

    private setTranslateFields() {

        this.translateService.get('CANCEL')
            .subscribe(value => { this.cancelButtonLabel = value; });
        this.translateService.get('ERROR')
            .subscribe(value => { this.errorAlertTitle = value })
        this.translateService.get('INFO')
            .subscribe(value => { this.okAlertTitle = value; });
        this.translateService.get('INFO')
            .subscribe(value => { this.yesNoAlertTitle = value; });
        this.translateService.get('NO')
            .subscribe(value => { this.noButtonLabel = value; });
        this.translateService.get('OK')
            .subscribe(value => { this.okButtonLabel = value; });
        this.translateService.get('YES')
            .subscribe(value => { this.yesButtonLabel = value; });
    }

    public errorAlert(message: string) {
        this.translateService.get(message).subscribe(translation => {
            let alert = this.alertController.create({
                title: this.errorAlertTitle,
                subTitle: translation,
                enableBackdropDismiss: false,
                buttons: [{ text: this.okButtonLabel }]
            });
            alert.present();
        });
    }

    public okAlert(message: string, okAction?: any) {
        this.translateService.get(message).subscribe(translation => {
            let alert = this.alertController.create({
                title: this.okAlertTitle,
                subTitle: translation,
                enableBackdropDismiss: false,
                buttons: [
                    { text: this.okButtonLabel, handler: data => { if (okAction) { okAction(); } } }
                ]
            });
            alert.present();
        });
    }

    public yesNoAlert(message: string, yesAction?: any, noAction?: any) {
        this.translateService.get(message).subscribe(translation => {
            let alert = this.alertController.create({
                title: this.yesNoAlertTitle,
                subTitle: translation,
                enableBackdropDismiss: false,
                buttons: [
                    { text: this.noButtonLabel, handler: data => { if (noAction) { noAction(); } } },
                    { text: this.yesButtonLabel, handler: data => { if (yesAction) { yesAction(); } } }
                ]
            });
            alert.present();
        });
    }
}
