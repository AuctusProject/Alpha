import { Component, OnInit } from '@angular/core';
import { CanActivate, Router, ActivatedRoute } from '@angular/router';
import { AccountService } from "../../services/account.service";
import { NotificationsService } from "angular2-notifications";
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  loading: boolean = true;
  success: boolean = false;
  constructor(private route: ActivatedRoute, 
              private accountService: AccountService,
              private notificationService: NotificationsService,
              private router: Router,
              private loginService: LoginService) { }

  ngOnInit() {
    this.confirmEmail();
  }

  confirmEmail() : void{
    let code = this.route.snapshot.queryParams['c'];
    this.accountService.confirmEmail(code).subscribe(
      response => {
        this.loading = false;
        if (response) {
          this.success = true;
          this.notificationService.success("Sucess", "Email confirmed!");
          if(this.loginService.isLoggedIn()){
            let loginData = this.loginService.getLoginData();
            loginData.pendingConfirmation = false;
            this.loginService.setLoginData(loginData);
            this.router.navigateByUrl('');
          }
          else{
            this.router.navigateByUrl('login');
          }
        }
      }
    );
  }

}
