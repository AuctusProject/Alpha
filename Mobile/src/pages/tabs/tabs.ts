import { Component } from '@angular/core';

import { ProjectionPage } from '../projection/projection';
import { PortfolioPage } from './../portfolio/portfolio';
import { HistoricalPage } from './../historical/historical';

@Component({
    templateUrl: 'tabs.html'
})
export class TabsPage {

    projectionTab = ProjectionPage;
    portfolioTab = PortfolioPage;
    historicalTab = HistoricalPage

    constructor() {
    }
}
