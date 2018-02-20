import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck, IconX } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck, IconX];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
