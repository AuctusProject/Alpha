import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioHistoryComponent } from './portfolio-history.component';

describe('PortfolioHistoryComponent', () => {
  let component: PortfolioHistoryComponent;
  let fixture: ComponentFixture<PortfolioHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
