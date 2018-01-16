import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalOptionComponent } from './goal-option.component';

describe('GoalOptionComponent', () => {
  let component: GoalOptionComponent;
  let fixture: ComponentFixture<GoalOptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoalOptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoalOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
