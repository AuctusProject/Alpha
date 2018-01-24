import { Component } from '@angular/core';

import { AboutPage } from '../about/about';
import { ContactPage } from '../contact/contact';
import { HomePage } from '../home/home';
import { ProjectionPage } from '../projection/projection';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  projectionTab = ProjectionPage;
  tab3Root = ContactPage;

  constructor() {
  }
}
