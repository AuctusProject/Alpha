import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioDistributionComponent } from './portfolio-distribution.component';

describe('PortfolioDistributionComponent', () => {
  let component: PortfolioDistributionComponent;
  let fixture: ComponentFixture<PortfolioDistributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioDistributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
