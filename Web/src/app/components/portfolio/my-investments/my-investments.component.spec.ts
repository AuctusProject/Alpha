import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyInvestmentsComponent } from './my-investments.component';

describe('MyInvestmentsComponent', () => {
  let component: MyInvestmentsComponent;
  let fixture: ComponentFixture<MyInvestmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyInvestmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyInvestmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
