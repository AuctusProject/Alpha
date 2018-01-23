import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvisorDetailComponent } from './advisor-detail.component';

describe('AdvisorDetailComponent', () => {
  let component: AdvisorDetailComponent;
  let fixture: ComponentFixture<AdvisorDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvisorDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvisorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
