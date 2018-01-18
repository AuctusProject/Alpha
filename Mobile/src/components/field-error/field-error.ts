import { Component, Input } from '@angular/core';


@Component({
  selector: 'field-error',
  templateUrl: 'field-error.html'
})
export class FieldErrorComponent {

  @Input() control: any;
  @Input() isSubmitted: boolean;

  constructor() {
  }

  private hasErrors() {
    return (this.isSubmitted || this.control.touched) && this.control.errors;
  }

  public isRequiredError(): boolean {
    return this.hasErrors() && this.control.errors.required;
  }

  public isEmailError(): boolean {
    return  this.hasErrors() && this.control.errors.email;
  }
}
