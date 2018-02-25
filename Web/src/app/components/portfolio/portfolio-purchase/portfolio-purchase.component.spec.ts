import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioPurchaseComponent } from './portfolio-purchase.component';

describe('PortfolioPurchaseComponent', () => {
  let component: PortfolioPurchaseComponent;
  let fixture: ComponentFixture<PortfolioPurchaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioPurchaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioPurchaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
