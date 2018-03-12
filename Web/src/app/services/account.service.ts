import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { GoalOption } from "../model/account/goalOption";
import { FullRegister } from "../model/account/fullRegister";
import { Goal } from '../model/account/goal';
import { SimpleRegister } from '../model/account/simpleRegister';
import { LoginResult } from '../model/account/loginResult';
import { UserBalance } from '../model/account/userBalance';
import { User } from '../model/account/user';

@Injectable()
export class AccountService {

  private listGoalOptionsUrl = this.httpService.apiUrl("accounts/v1/goals/options");
  private fullRegisterUrl = this.httpService.apiUrl("accounts/v1/registration/full");
  private simpleRegisterUrl = this.httpService.apiUrl("accounts/v1/registration/simple");
  private setGoalUrl = this.httpService.apiUrl("accounts/v1/goals");
  private checkTelegramUrl = this.httpService.apiUrl("accounts/v1/checkTelegram");
  private confirmEmailUrl = this.httpService.apiUrl("accounts/v1/confirmation");
  private forgotPasswordUrl = this.httpService.apiUrl("accounts/v1/password/forgotten");
  private resetPasswordUrl = this.httpService.apiUrl("accounts/v1/password/recovery");
  private resendConfirmationUrl = this.httpService.apiUrl("accounts/v1/confirmation/resend");
  private changePasswordUrl = this.httpService.apiUrl("accounts/v1/password/change");
  private generateApiKeyUrl = this.httpService.apiUrl("accounts/v1/apikeys");
  private getLastApiKeyUrl = this.httpService.apiUrl("accounts/v1/apikeys/last");
  private revokeApiKeyUrl = this.httpService.apiUrl("accounts/v1/apikeys");
  private validateEmailUrl = this.httpService.apiUrl("accounts/v1/emails");
  private balanceFromCacheUrl = this.httpService.apiUrl("accounts/v1/balanceFromCache");
  private balanceUrl = this.httpService.apiUrl("accounts/v1/balance");
  private listUsersByPerformanceUrl = this.httpService.apiUrl("accounts/v1/performance");
  private validateUsernameUrl = this.httpService.apiUrl("accounts/v1/usernames");
  private faucetUrl = this.httpService.apiUrl("accounts/v1/faucet");

  constructor(private httpService: HttpService) { }

  listGoalOptions(): Observable<GoalOption[]> {
    return this.httpService.get(this.listGoalOptionsUrl);
  }

  fullRegister(fullRegisterDTO: FullRegister): Observable<FullRegister> {
    return this.httpService.post(this.fullRegisterUrl, fullRegisterDTO);
  }

  simpleRegister(simpleRegisterDTO: SimpleRegister): Observable<LoginResult> {
    return this.httpService.post(this.simpleRegisterUrl, simpleRegisterDTO);
  }

  checkTelegram(phoneNumber: string) {
    return this.httpService.get(this.checkTelegramUrl + "/" + phoneNumber);
  }

  setGoal(goalDTO: Goal): Observable<Goal> {
    return this.httpService.post(this.setGoalUrl, goalDTO);
  }

  confirmEmail(code: string): Observable<any> {
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

  getUserBalance(): Observable<UserBalance> {
    return this.httpService.get(this.balanceUrl);
  }

  listUsersByPerformance(): Observable<any[]> {
    return this.httpService.get(this.listUsersByPerformanceUrl);
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    let changePasswordRequest = {
      CurrentPassword: currentPassword,
      NewPassword: newPassword
    }
    return this.httpService.post(this.changePasswordUrl, changePasswordRequest);
  }

  faucet(address: string): Observable<any> {
    return this.httpService.post(this.faucetUrl, {Address: address});
  }

  generateApiKey(): Observable<any> {
    return this.httpService.post(this.generateApiKeyUrl, null);
  }

  getLastApiKey(): Observable<any> {
    return this.httpService.get(this.getLastApiKeyUrl);
  }

  revokeApiKey(): Observable<any> {
    return this.httpService.delete(this.revokeApiKeyUrl);
  }

  validateEmail(email: string): Observable<any> {
    return this.httpService.get(this.validateEmailUrl + "/" + email);
  }

  validateUsername(username: string): Observable<any> {
    return this.httpService.get(this.validateUsernameUrl + "/" + username);
  }

}
