import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoboAdvisorsComponent } from './robo-advisors.component';

describe('RoboAdvisorsComponent', () => {
  let component: RoboAdvisorsComponent;
  let fixture: ComponentFixture<RoboAdvisorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoboAdvisorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoboAdvisorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
