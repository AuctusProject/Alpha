import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router/src/router_state';

@Injectable()
export class UserProfileAuthGuard implements CanActivate {

  constructor(private loginService: LoginService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let logged = this.loginService.isLoggedIn();
    if (logged) {

      let loginData = this.loginService.getLoginData();

      if (loginData.hasInvestment) {
        this.router.navigateByUrl('dashboard');
      } else if (loginData.humanAdvisorId) {
        this.router.navigateByUrl('advisor/' + loginData.humanAdvisorId);
      } else {
        this.router.navigateByUrl('human-advisors');
      }

    }

    return !logged;
  }
}
