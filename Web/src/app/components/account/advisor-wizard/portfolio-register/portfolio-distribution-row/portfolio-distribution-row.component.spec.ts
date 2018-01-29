import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioDistributionRowComponent } from './portfolio-distribution-row.component';

describe('PortfolioDistributionRowComponent', () => {
  let component: PortfolioDistributionRowComponent;
  let fixture: ComponentFixture<PortfolioDistributionRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioDistributionRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioDistributionRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
