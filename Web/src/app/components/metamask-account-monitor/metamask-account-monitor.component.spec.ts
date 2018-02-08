import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MetamaskAccountMonitorComponent } from './metamask-account-monitor.component';

describe('MetamaskAccountMonitorComponent', () => {
  let component: MetamaskAccountMonitorComponent;
  let fixture: ComponentFixture<MetamaskAccountMonitorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MetamaskAccountMonitorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MetamaskAccountMonitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
