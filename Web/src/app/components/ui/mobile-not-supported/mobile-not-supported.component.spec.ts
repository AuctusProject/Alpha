import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MobileNotSupportedComponent } from './mobile-not-supported.component';

describe('MobileNotSupportedComponent', () => {
  let component: MobileNotSupportedComponent;
  let fixture: ComponentFixture<MobileNotSupportedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MobileNotSupportedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MobileNotSupportedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
