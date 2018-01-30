
import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule } from '@angular/common/http';

import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { MyApp } from './app.component';
import { EvolutionPage } from '../pages/evolution/evolution';
import { HistoricalPage } from '../pages/historical/historical';
import { HomePage } from '../pages/home/home';
import { LoginPage } from './../pages/login/login';
import { ProjectionPage } from '../pages/projection/projection';
import { PurchaseSelectPage } from '../pages/purchase-select/purchase-select';
import { PortfolioPage } from './../pages/portfolio/portfolio';
import { TabsPage } from '../pages/tabs/tabs';

import { FieldErrorComponent } from './../components/field-error/field-error';
import { NavbarComponent } from './../components/navbar/navbar';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { ChartistModule } from 'angular2-chartist';
import { ChartsModule } from 'ng2-charts';

import { AccountService } from './../services/account.service';
import { AdviceService } from './../services/advice.service';
import { BaseService } from './../services/base.service';
import { PortfolioService } from '../services/portfolio.service';

import { AlertHelper } from './../helpers/alert-helper';
import { LoadingHelper } from '../helpers/loading-helper';
import { StorageHelper } from '../helpers/storage-helper';


export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
    declarations: [
        MyApp,
        EvolutionPage,
        HistoricalPage,
        HomePage,
        LoginPage,
        PortfolioPage,
        ProjectionPage,
        PurchaseSelectPage,
        TabsPage,

        FieldErrorComponent,
        NavbarComponent
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
        EvolutionPage,
        HistoricalPage,
        HomePage,
        LoginPage,
        PortfolioPage,
        ProjectionPage,
        PurchaseSelectPage,
        TabsPage,

        FieldErrorComponent,
        NavbarComponent
    ],
    providers: [
        StatusBar,
        SplashScreen,

        AccountService,
        AdviceService,
        BaseService,
        PortfolioService,

        AlertHelper,
        LoadingHelper,
        StorageHelper,

        { provide: ErrorHandler, useClass: IonicErrorHandler }
    ]
})
export class AppModule { }
