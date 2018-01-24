import { Injector, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { AlertHelper } from '../helpers/alert-helper'
import { LoadingHelper } from './../helpers/loading-helper';
import { StorageHelper } from '../helpers/storage-helper';

import { TranslateService } from '@ngx-translate/core';

import { NavController, NavParams, ModalController, ViewController, ActionSheetController } from 'ionic-angular';

import * as _ from 'lodash';
import moment from 'moment';

export class BasePage {

    public isSubmittedForm: boolean;

    //@angular/core
    protected chRef: ChangeDetectorRef;

    //@angular/forms
    protected formBuilder: FormBuilder;
    protected validators: any;

    //Helpers
    protected alertHelper: AlertHelper;
    public loadingHelper: LoadingHelper;
    protected storageHelper: StorageHelper

    //ionic-angular
    protected actionSheetCtrl: ActionSheetController;
    protected modalCtrl: ModalController;
    protected navCtrl: NavController;
    protected navParams: NavParams;
    protected viewCtrl: ViewController;

    //lodash
    protected lodash: any;

    protected moment: any;

    protected translateService: TranslateService

    constructor(protected injector: Injector) {

        //@angular/core
        this.chRef = this.injector.get(ChangeDetectorRef);

        //@angular/forms
        this.formBuilder = this.injector.get(FormBuilder);
        this.validators = Validators;

        //Helpers
        this.alertHelper = this.injector.get(AlertHelper);
        this.loadingHelper = this.injector.get(LoadingHelper);
        this.storageHelper = this.injector.get(StorageHelper);

        //ionic-angular
        this.modalCtrl = this.injector.get(ModalController);
        this.navCtrl = this.injector.get(NavController);
        this.navParams = this.injector.get(NavParams);
        this.viewCtrl = this.injector.get(ViewController);
        this.actionSheetCtrl = this.injector.get(ActionSheetController);

        //lodash
        this.lodash = _;

        this.moment = moment;

        this.translateService = this.injector.get(TranslateService);
    }

    protected closeView() {
        this.navCtrl.pop()
    }

}
