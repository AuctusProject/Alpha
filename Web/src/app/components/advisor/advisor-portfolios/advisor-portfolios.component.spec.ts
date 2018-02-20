import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvisorPortfoliosComponent } from './advisor-portfolios.component';

describe('AdvisorPortfoliosComponent', () => {
  let component: AdvisorPortfoliosComponent;
  let fixture: ComponentFixture<AdvisorPortfoliosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvisorPortfoliosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvisorPortfoliosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
