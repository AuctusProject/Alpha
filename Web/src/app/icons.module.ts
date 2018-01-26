import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle, IconEdit } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle, IconEdit];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
