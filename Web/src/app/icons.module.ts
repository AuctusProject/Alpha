import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
