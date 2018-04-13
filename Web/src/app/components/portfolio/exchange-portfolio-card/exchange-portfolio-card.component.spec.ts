import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangePortfolioCardComponent } from './exchange-portfolio-card.component';

describe('ExchangePortfolioCardComponent', () => {
  let component: ExchangePortfolioCardComponent;
  let fixture: ComponentFixture<ExchangePortfolioCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExchangePortfolioCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExchangePortfolioCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
