import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NameStepComponent } from './name-step.component';

describe('NameStepComponent', () => {
  let component: NameStepComponent;
  let fixture: ComponentFixture<NameStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NameStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NameStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
