import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PercentageLabelComponent } from './percentage-label.component';

describe('PercentageLabelComponent', () => {
  let component: PercentageLabelComponent;
  let fixture: ComponentFixture<PercentageLabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PercentageLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PercentageLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
