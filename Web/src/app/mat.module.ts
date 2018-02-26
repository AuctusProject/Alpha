import { NgModule } from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatButtonToggleModule, MatDatepickerModule, MatNativeDateModule } from '@angular/material';
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
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from "@angular/material";

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
    MatMenuModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule
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
    MatMenuModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule
    
  ],
  declarations: []
})
export class MatModule { }
