import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectionTabComponent } from './projection-tab.component';

describe('ProjectionTabComponent', () => {
  let component: ProjectionTabComponent;
  let fixture: ComponentFixture<ProjectionTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectionTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectionTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
