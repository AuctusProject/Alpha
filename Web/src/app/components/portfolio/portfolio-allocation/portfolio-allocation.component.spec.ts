import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioAllocationComponent } from './portfolio-allocation.component';

describe('PortfolioAllocationComponent', () => {
  let component: PortfolioAllocationComponent;
  let fixture: ComponentFixture<PortfolioAllocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioAllocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioAllocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
