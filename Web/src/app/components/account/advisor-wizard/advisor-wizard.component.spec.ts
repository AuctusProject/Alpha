import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvisorWizardComponent } from './advisor-wizard.component';

describe('AdvisorWizardComponent', () => {
  let component: AdvisorWizardComponent;
  let fixture: ComponentFixture<AdvisorWizardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvisorWizardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvisorWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
