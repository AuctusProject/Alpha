import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { GoalOption } from "../model/account/goalOption";
import { FullRegister } from "../model/account/fullRegister";
import { Goal } from '../model/account/goal';

@Injectable()
export class AccountService {

  private listGoalOptionsUrl = this.httpService.apiUrl("accounts/v1/goals/options");
  private fullRegisterUrl = this.httpService.apiUrl("accounts/v1/registration/full");
  private setGoalUrl = this.httpService.apiUrl("accounts/v1/goals");
  private confirmEmailUrl = this.httpService.apiUrl("accounts/v1/confirmation");
  private forgotPasswordUrl = this.httpService.apiUrl("accounts/v1/password/forgotten");
  private resetPasswordUrl = this.httpService.apiUrl("accounts/v1/password/recovery");
  private resendConfirmationUrl = this.httpService.apiUrl("accounts/v1/confirmation/resend");
  private changePasswordUrl = this.httpService.apiUrl("accounts/v1/password/change");

  constructor(private httpService : HttpService) { }

  listGoalOptions(): Observable<GoalOption[]> {
    return this.httpService.get(this.listGoalOptionsUrl);
  }

  fullRegister(fullRegisterDTO : FullRegister): Observable<FullRegister> {
    return this.httpService.post(this.fullRegisterUrl, fullRegisterDTO);
  }

  setGoal(goalDTO : Goal): Observable<Goal> {
    return this.httpService.post(this.setGoalUrl, goalDTO);
  }

  confirmEmail(code : string): Observable<any> {
    let confirmationRequest = {
      Code: code
    }
    return this.httpService.post(this.confirmEmailUrl, confirmationRequest);
  }

  recoverPassword(emailAccount: string): Observable<any> {
    let recoverPasswordRequest = {
      Email: emailAccount
    }
    return this.httpService.post(this.forgotPasswordUrl, recoverPasswordRequest);
  }

  resetPassword(code: string, password: string): Observable<any> {
    let resetPasswordRequest = {
      Code: code,
      Password: password
    }
    return this.httpService.post(this.resetPasswordUrl, resetPasswordRequest);
  }

  resendConfirmation(email: string): Observable<any> {
    let resendConfirmationRequest = {
      Email: email
    }
    return this.httpService.post(this.resendConfirmationUrl, resendConfirmationRequest);
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    let changePasswordRequest = {
      CurrentPassword: currentPassword,
      NewPassword: newPassword
    }
    return this.httpService.post(this.changePasswordUrl, changePasswordRequest);
  }
}
