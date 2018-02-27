import { Component, OnInit, Input } from '@angular/core';
import { Login } from '../../../model/account/login';
import { LoginResult } from '../../../model/account/loginResult';
import { LoginService } from '../../../services/login.service';
import { NotificationsService } from "angular2-notifications";
import { Router } from '@angular/router';
import { Web3Service } from '../../../services/web3.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { EventsService } from "angular-event-service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  pendingConfirmation: boolean;
  @Input() login: Login = new Login();

  public loginForm: FormGroup;
  loginPromise: Subscription;

  constructor(
    private eventsService: EventsService,
    private formBuilder: FormBuilder,
    private loginService: LoginService, 
    private notificationService: NotificationsService, 
    private router: Router,
    private web3Service: Web3Service) { 
      this.buildForm();
    }

    private buildForm() {
      this.loginForm = this.formBuilder.group({
        emailOrUsername: ['', Validators.compose([Validators.required])],
        password: ['', Validators.compose([Validators.required])],
        address: ['']
      });
    }

  ngOnInit() {
    this.pendingConfirmation = false;
    this.eventsService.on("loginConditionsSuccess", this.onLoginConditionsSuccess);
    
  }

  private onLoginConditionsSuccess: Function = (payload: any) => {
    this.web3Service.getAccount().subscribe(address => {
      this.login.address = address;
    });
  }

  public onLoginClick() {
    if(this.loginForm.valid){
      this.doLogin();
    }
  }

  doLogin() {
    this.loginPromise = this.loginService.login(this.login)
      .subscribe(response => {
        if (response){
          if (response.logged) {
            this.loginService.setLoginData(response.data);
            this.router.navigateByUrl('');
          }
          else {
            this.pendingConfirmation = true;
            this.notificationService.info("Info", response.error);
          }
        }
      });
  }
}
