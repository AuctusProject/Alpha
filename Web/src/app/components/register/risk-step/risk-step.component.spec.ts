import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskStepComponent } from './risk-step.component';

describe('RiskStepComponent', () => {
  let component: RiskStepComponent;
  let fixture: ComponentFixture<RiskStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
