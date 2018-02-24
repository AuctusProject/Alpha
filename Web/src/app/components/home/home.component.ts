import { NotificationsService } from 'angular2-notifications';
import { SimpleRegister } from './../../model/account/simpleRegister';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { Web3Service } from '../../services/web3.service';
import { MetamaskAccountService } from '../../services/metamask-account.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  show: boolean;
  public simpleRegisterForm: FormGroup;
  public simpleRegister: SimpleRegister;
  createPromise: Subscription;

  constructor(private formBuilder: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private notificationService: NotificationsService,
    private web3Service: Web3Service,
    private metamaskAccountService : MetamaskAccountService) {
    this.simpleRegister = new SimpleRegister();
    this.buildForm();
  }

  ngOnInit() {

  }

  private buildForm() {
    this.simpleRegisterForm = this.formBuilder.group({
      username: ['', Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(30)])],
      email: ['', Validators.compose([Validators.email, Validators.required])],
      password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(100)])],
    });
  }

  public onSubmit() {
    this.createPromise = this.web3Service.getAccount().subscribe(success => {
      this.simpleRegister.address = success;
      this.createAccount();
    });

  }

  private createAccount() {
      this.createPromise = this.accountService.simpleRegister(this.simpleRegister).subscribe(success => {
        this.router.navigateByUrl('');
      }, response => {
        this.notificationService.info("Info", response.error);
      });
  }

  public getErrorMessage(formField: any) {
    if (formField.hasError('required')) {
      return 'Required field';
    } else if (formField.hasError('email')) {
      return 'Not a valid email';
    } else if (formField.hasError('minlength')) {
      return 'Field must be at least ' + formField.errors.minlength.requiredLength + ' characters long.'
    } else if (formField.hasError('maxlength')) {
      return 'Field can be max ' + formField.errors.maxlength.requiredLength + ' characters long.'
    } else if (formField.hasError('pattern')) {
      return 'Field must be only [a-zA-Z-_.] characters'
    } else if (formField.hasError('emailRegistration')) {
      return 'Email already registered.'
    } else if (formField.hasError('usernameRegistration')) {
      return 'Username already registered.'
    }
  }

  public purchase(){
    this.metamaskAccountService.sendAUC(100);
  }
}
