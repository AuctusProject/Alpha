import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioPurchasePopupComponent } from './portfolio-purchase-popup.component';

describe('PortfolioPurchasePopupComponent', () => {
  let component: PortfolioPurchasePopupComponent;
  let fixture: ComponentFixture<PortfolioPurchasePopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioPurchasePopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioPurchasePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
