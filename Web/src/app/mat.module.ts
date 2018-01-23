import { NgModule } from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatButtonToggleModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';
import { MatSliderModule } from '@angular/material';
import { MatToolbarModule } from '@angular/material';
import { MatIconModule } from '@angular/material';
import { MatGridListModule, } from '@angular/material';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatMenuModule } from "@angular/material";


@NgModule({
  imports: [
    MatButtonModule, 
    MatCheckboxModule,
    MatButtonToggleModule, 
    MatStepperModule, 
    MatFormFieldModule,
    MatInputModule, 
    MatSliderModule, 
    MatToolbarModule,
    MatIconModule,
    MatGridListModule,
    MatTabsModule,
    MatTableModule,
    MatMenuModule
  ],
  exports: [
    MatButtonModule, 
    MatCheckboxModule, 
    MatButtonToggleModule, 
    MatStepperModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatSliderModule, 
    MatToolbarModule,
    MatIconModule,
    MatGridListModule,
    MatTabsModule,
    MatTableModule,
    MatMenuModule
  ],
  declarations: []
})
export class MatModule { }
