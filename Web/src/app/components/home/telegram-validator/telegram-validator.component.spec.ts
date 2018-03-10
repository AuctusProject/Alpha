import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TelegramValidatorComponent } from './telegram-validator.component';

describe('TelegramValidatorComponent', () => {
  let component: TelegramValidatorComponent;
  let fixture: ComponentFixture<TelegramValidatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TelegramValidatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TelegramValidatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
