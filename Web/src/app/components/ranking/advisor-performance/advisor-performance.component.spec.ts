import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvisorPerformanceComponent } from './advisor-performance.component';

describe('AdvisorPerformanceComponent', () => {
  let component: AdvisorPerformanceComponent;
  let fixture: ComponentFixture<AdvisorPerformanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvisorPerformanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvisorPerformanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
