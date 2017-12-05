import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AmountStepComponent } from './amount-step.component';

describe('AmountStepComponent', () => {
  let component: AmountStepComponent;
  let fixture: ComponentFixture<AmountStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AmountStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AmountStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
