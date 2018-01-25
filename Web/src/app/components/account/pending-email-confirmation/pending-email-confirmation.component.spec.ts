import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingEmailConfirmationComponent } from './pending-email-confirmation.component';

describe('PendingEmailConfirmationComponent', () => {
  let component: PendingEmailConfirmationComponent;
  let fixture: ComponentFixture<PendingEmailConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PendingEmailConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingEmailConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
