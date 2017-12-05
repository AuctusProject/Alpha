import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PeriodStepComponent } from './period-step.component';

describe('PeriodStepComponent', () => {
  let component: PeriodStepComponent;
  let fixture: ComponentFixture<PeriodStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeriodStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeriodStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
