import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalHeaderComponent } from './goal-header.component';

describe('GoalHeaderComponent', () => {
  let component: GoalHeaderComponent;
  let fixture: ComponentFixture<GoalHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoalHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoalHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
