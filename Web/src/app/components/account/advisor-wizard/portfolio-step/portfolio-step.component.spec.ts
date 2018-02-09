import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioStepComponent } from './portfolio-step.component';

describe('PortfolioStepComponent', () => {
  let component: PortfolioStepComponent;
  let fixture: ComponentFixture<PortfolioStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
