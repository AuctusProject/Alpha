import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordEmailComponent } from './forgot-password-email.component';

describe('ForgotPasswordEmailComponent', () => {
  let component: ForgotPasswordEmailComponent;
  let fixture: ComponentFixture<ForgotPasswordEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordEmailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
