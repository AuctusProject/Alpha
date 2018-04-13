import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangePortfolioDetailsComponent } from './exchange-portfolio-details.component';

describe('ExchangePortfolioDetailsComponent', () => {
  let component: ExchangePortfolioDetailsComponent;
  let fixture: ComponentFixture<ExchangePortfolioDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExchangePortfolioDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExchangePortfolioDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
