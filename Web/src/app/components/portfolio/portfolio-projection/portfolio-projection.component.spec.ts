import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioProjectionComponent } from './portfolio-projection.component';

describe('PortfolioProjectionComponent', () => {
  let component: PortfolioProjectionComponent;
  let fixture: ComponentFixture<PortfolioProjectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioProjectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioProjectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
