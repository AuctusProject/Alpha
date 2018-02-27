import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioProjectionChartComponent } from './portfolio-projection-chart.component';

describe('PortfolioProjectionChartComponent', () => {
  let component: PortfolioProjectionChartComponent;
  let fixture: ComponentFixture<PortfolioProjectionChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioProjectionChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioProjectionChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
