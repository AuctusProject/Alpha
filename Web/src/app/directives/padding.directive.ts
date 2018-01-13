import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appPadding]'
})
export class PaddingDirective {

  @Input('appPadding') paddingValue: string;

  constructor(el: ElementRef) {
    el.nativeElement.style.padding = this.paddingValue;
  }

}
