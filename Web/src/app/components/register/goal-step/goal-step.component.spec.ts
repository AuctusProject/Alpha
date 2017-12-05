import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalStepComponent } from './goal-step.component';

describe('GoalStepComponent', () => {
  let component: GoalStepComponent;
  let fixture: ComponentFixture<GoalStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoalStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoalStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
