import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageApiComponent } from './manage-api.component';

describe('ManageApiComponent', () => {
  let component: ManageApiComponent;
  let fixture: ComponentFixture<ManageApiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageApiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageApiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
