import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioHistoryChartComponent } from './portfolio-history-chart.component';

describe('PortfolioHistoryChartComponent', () => {
  let component: PortfolioHistoryChartComponent;
  let fixture: ComponentFixture<PortfolioHistoryChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioHistoryChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioHistoryChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
