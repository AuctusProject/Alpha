import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncExchangeComponent } from './sync-exchange.component';

describe('SyncExchangeComponent', () => {
  let component: SyncExchangeComponent;
  let fixture: ComponentFixture<SyncExchangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SyncExchangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SyncExchangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
