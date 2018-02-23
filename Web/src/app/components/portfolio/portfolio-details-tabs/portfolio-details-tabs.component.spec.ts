import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioDetailsTabsComponent } from './portfolio-details-tabs.component';

describe('PortfolioDetailsTabsComponent', () => {
  let component: PortfolioDetailsTabsComponent;
  let fixture: ComponentFixture<PortfolioDetailsTabsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioDetailsTabsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioDetailsTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
