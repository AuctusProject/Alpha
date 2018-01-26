import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EthMachineComponent } from './eth-machine.component';

describe('EthMachineComponent', () => {
  let component: EthMachineComponent;
  let fixture: ComponentFixture<EthMachineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EthMachineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EthMachineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
