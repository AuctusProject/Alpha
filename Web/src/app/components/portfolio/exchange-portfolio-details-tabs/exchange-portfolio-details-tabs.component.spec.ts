import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangePortfolioDetailsTabsComponent } from './exchange-portfolio-details-tabs.component';

describe('ExchangePortfolioDetailsTabsComponent', () => {
  let component: ExchangePortfolioDetailsTabsComponent;
  let fixture: ComponentFixture<ExchangePortfolioDetailsTabsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExchangePortfolioDetailsTabsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExchangePortfolioDetailsTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
