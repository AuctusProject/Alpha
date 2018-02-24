import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioReturnIndicatorComponent } from './portfolio-return-indicator.component';

describe('PortfolioReturnIndicatorComponent', () => {
  let component: PortfolioReturnIndicatorComponent;
  let fixture: ComponentFixture<PortfolioReturnIndicatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioReturnIndicatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioReturnIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
