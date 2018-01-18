import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule } from '@angular/common/http';

import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';


import { MyApp } from './app.component';

import { AboutPage } from '../pages/about/about';
import { ContactPage } from '../pages/contact/contact';
import { HomePage } from '../pages/home/home';
import { LoginPage } from './../pages/login/login';
import { TabsPage } from '../pages/tabs/tabs';

import { AsyncChartComponent } from '../pages/components/async-chart.component';
import { DynamicChartComponent } from '../pages/components/dynamic-chart.component';
import { LiveChartComponent } from '../pages/components/live-chart.component';
import { FieldErrorComponent } from './../components/field-error/field-error';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { ChartistModule } from 'angular2-chartist';
import { ChartsModule } from 'ng2-charts';


import { AccountService } from './../services/account.service';
import { BaseService } from './../services/base.service';

import { AlertHelper } from './../helpers/alert-helper';
import { LoadingHelper } from '../helpers/loading-helper';
import { StorageHelper } from '../helpers/storage-helper';


export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    MyApp,
    AboutPage,
    ContactPage,
    HomePage,
    LoginPage,
    TabsPage,

    AsyncChartComponent,
    DynamicChartComponent,
    FieldErrorComponent,
    LiveChartComponent
  ],
  imports: [
    BrowserModule,
    ChartistModule,
    ChartsModule,
    HttpClientModule,
    IonicModule.forRoot(MyApp),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    AboutPage,
    ContactPage,
    HomePage,
    LoginPage,
    TabsPage,

    AsyncChartComponent,
    DynamicChartComponent,
    FieldErrorComponent,
    LiveChartComponent
  ],
  providers: [
    StatusBar,
    SplashScreen,

    AccountService,
    BaseService,

    AlertHelper,
    LoadingHelper,
    StorageHelper,

    { provide: ErrorHandler, useClass: IonicErrorHandler }
  ]
})
export class AppModule { }
