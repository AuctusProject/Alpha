import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountHeaderComponent } from './account-header.component';

describe('AccountHeaderComponent', () => {
  let component: AccountHeaderComponent;
  let fixture: ComponentFixture<AccountHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
