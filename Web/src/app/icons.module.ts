import { NgModule } from '@angular/core';
import { IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck, IconX, IconArrowRight, IconHelpCircle, IconArrowDown, IconArrowUp, IconMinus } from 'angular-feather';


var icons = [IconEye, IconEyeOff, IconCheckCircle, IconEdit, IconChevronDown, IconPlusCircle, IconMinusCircle, IconPlus, IconSave, IconAlertTriangle, IconCheck, IconX, IconArrowRight, IconHelpCircle, IconArrowDown, IconArrowUp, IconMinus];

@NgModule({
  imports: icons,
  exports: icons
})
export class IconsModule { }
