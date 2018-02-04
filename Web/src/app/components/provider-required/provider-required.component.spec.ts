import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderRequiredComponent } from './provider-required.component';

describe('ProviderRequiredComponent', () => {
  let component: ProviderRequiredComponent;
  let fixture: ComponentFixture<ProviderRequiredComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProviderRequiredComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProviderRequiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
