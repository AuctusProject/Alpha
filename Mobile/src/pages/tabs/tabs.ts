import { Component } from '@angular/core';

import { ProjectionPage } from '../projection/projection';
import { PortifolioPage } from './../portifolio/portifolio';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  projectionTab = ProjectionPage;
  portifolioTab = PortifolioPage;

  constructor() {
  }
}
