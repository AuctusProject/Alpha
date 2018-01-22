import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioHistogramComponent } from './portfolio-histogram.component';

describe('PortfolioHistogramComponent', () => {
  let component: PortfolioHistogramComponent;
  let fixture: ComponentFixture<PortfolioHistogramComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioHistogramComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioHistogramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
