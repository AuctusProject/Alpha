import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
