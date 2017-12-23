import { Directive, OnInit, ElementRef, Renderer, Input } from '@angular/core';

@Directive({
  selector: '[inputFocus]'
})
export class FocusDirective implements OnInit {
  
   @Input('inputFocus') isFocused: boolean;
  
   constructor(private hostElement: ElementRef, private renderer: Renderer) {}
  
   ngOnInit() {
     if (this.isFocused) {
       this.renderer.invokeElementMethod(this.hostElement.nativeElement, 'focus');
     }
   }
 }
