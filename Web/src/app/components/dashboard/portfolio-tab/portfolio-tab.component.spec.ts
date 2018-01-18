import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioTabComponent } from './portfolio-tab.component';

describe('PortfolioTabComponent', () => {
  let component: PortfolioTabComponent;
  let fixture: ComponentFixture<PortfolioTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
