import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvisorStepComponent } from './advisor-step.component';

describe('AdvisorStepComponent', () => {
  let component: AdvisorStepComponent;
  let fixture: ComponentFixture<AdvisorStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvisorStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvisorStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
