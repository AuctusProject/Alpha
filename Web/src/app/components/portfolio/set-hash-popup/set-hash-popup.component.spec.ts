import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetHashPopupComponent } from './set-hash-popup.component';

describe('SetHashPopupComponent', () => {
  let component: SetHashPopupComponent;
  let fixture: ComponentFixture<SetHashPopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetHashPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetHashPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
