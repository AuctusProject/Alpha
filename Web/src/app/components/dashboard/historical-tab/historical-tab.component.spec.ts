import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricalTabComponent } from './historical-tab.component';

describe('HistoricalTabComponent', () => {
  let component: HistoricalTabComponent;
  let fixture: ComponentFixture<HistoricalTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistoricalTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricalTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
