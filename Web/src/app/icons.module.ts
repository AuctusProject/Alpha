import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
