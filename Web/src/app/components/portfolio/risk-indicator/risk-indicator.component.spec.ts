import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskIndicatorComponent } from './risk-indicator.component';

describe('RiskIndicatorComponent', () => {
  let component: RiskIndicatorComponent;
  let fixture: ComponentFixture<RiskIndicatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskIndicatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
