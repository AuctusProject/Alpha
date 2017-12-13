import { NgModule } from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatButtonToggleModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';


@NgModule({
  imports: [
    MatButtonModule, MatCheckboxModule, MatButtonToggleModule, MatStepperModule, MatFormFieldModule, MatInputModule
  ],
  exports: [
    MatButtonModule, MatCheckboxModule, MatButtonToggleModule, MatStepperModule, MatFormFieldModule, MatInputModule
  ],
  declarations: []
})
export class MatModule { }
