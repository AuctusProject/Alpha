import { Directive } from '@angular/core';
import { NG_ASYNC_VALIDATORS, FormControl, Validator, ValidationErrors } from '@angular/forms';

import { AccountService } from './../services/account.service';

@Directive({
  selector: '[usernameRegistration]',
  providers: [{ provide: NG_ASYNC_VALIDATORS, useExisting: UsernameRegistrationDirective, multi: true }]
})
export class UsernameRegistrationDirective implements Validator {

  isInvalid: boolean;

  message = { usernameRegistration: true };

  validate(c: FormControl):ValidationErrors {

    this.accountService.validateUsername(c.value.trim()).subscribe(response => {
      this.isInvalid = response && !response.isValid;
    });

    return new Promise(resolve => {
      setTimeout(() => {
        resolve(this.isInvalid ? this.message : null);
      }, 1000);
    });
  }

  constructor(private accountService: AccountService) {
  }
}
