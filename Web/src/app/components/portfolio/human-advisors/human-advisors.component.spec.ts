import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HumanAdvisorsComponent } from './human-advisors.component';

describe('HumanAdvisorsComponent', () => {
  let component: HumanAdvisorsComponent;
  let fixture: ComponentFixture<HumanAdvisorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HumanAdvisorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HumanAdvisorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
