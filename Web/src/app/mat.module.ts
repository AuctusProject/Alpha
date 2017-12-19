import { NgModule } from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatButtonToggleModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';
import { MatSliderModule } from '@angular/material';


@NgModule({
  imports: [
    MatButtonModule, MatCheckboxModule, MatButtonToggleModule, MatStepperModule, MatFormFieldModule, MatInputModule, MatSliderModule
  ],
  exports: [
    MatButtonModule, MatCheckboxModule, MatButtonToggleModule, MatStepperModule, MatFormFieldModule, MatInputModule, MatSliderModule
  ],
  declarations: []
})
export class MatModule { }
