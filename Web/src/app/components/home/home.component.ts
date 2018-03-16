import { NotificationsService } from "angular2-notifications";
import { SimpleRegister } from "./../../model/account/simpleRegister";
import { Component, OnInit, NgZone } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  Validators,
  AbstractControl,
  ValidatorFn
} from "@angular/forms";
import { AccountService } from "../../services/account.service";
import { Router } from "@angular/router";
import { Web3Service } from "../../services/web3.service";
import { MetamaskAccountService } from "../../services/metamask-account.service";
import { LoginService } from "../../services/login.service";
import { Subscription } from "rxjs/Subscription";
import { EventsService } from "angular-event-service";
import {
  MatDialog,
  MatDialogConfig,
  MatTableDataSource
} from "@angular/material";
import { TelegramValidatorComponent } from "./telegram-validator/telegram-validator.component";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit {
  show: boolean;
  public simpleRegisterForm: FormGroup;
  public simpleRegister: SimpleRegister;
  createPromise: Subscription;
  telegramDialog;
  showLoading: boolean = false;

  constructor(
    private eventsService: EventsService,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private loginService: LoginService,
    private router: Router,
    private notificationService: NotificationsService,
    private web3Service: Web3Service,
    private metamaskAccountService: MetamaskAccountService,
    private zone: NgZone,
    private dialog: MatDialog
  ) {
    this.simpleRegister = new SimpleRegister();
    this.buildForm();
  }

  ngOnInit() {
    if (this.metamaskAccountService.getAccount()) {
      this.eventsService.on(
        "loginConditionsSuccess",
        this.onLoginConditionsSuccess
      );
    } else {
      this.metamaskAccountService.broadcastLoginConditionsFail();
    }
  }

  private onLoginConditionsSuccess: Function = (payload: any) => {
    this.web3Service.getAccount().subscribe(address => {
      this.simpleRegister.address = address;
    });
  };

  private buildForm() {
    this.simpleRegisterForm = this.formBuilder.group({
      username: [
        "",
        Validators.compose([
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(30)
        ])
      ],
      email: ["", Validators.compose([Validators.email, Validators.required])],
      password: [
        "",
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(100)
        ])
      ],
      address: [""]
    });
  }

  public onSubmit() {
    this.createPromise = this.accountService
      .validateRegister(this.simpleRegister)
      .subscribe(result => {
        this.openTelegramDialog();
      });
  }

  public openTelegramDialog() {
    const dialogConfig = new MatDialogConfig();

    //dialogConfig.autoFocus = true;

    dialogConfig.data = {
      phoneNumber: "",
      homeComponent: this,
      onSuccess: this.onTelegramValidated
    };

    this.telegramDialog = this.dialog.open(
      TelegramValidatorComponent,
      dialogConfig
    );
  }

  private onTelegramValidated(homeComponent, result) {
    homeComponent.simpleRegister.phoneNumber = result;
    homeComponent.showLoading = true;
    homeComponent.createPromise = homeComponent.createAccount();
    homeComponent.telegramDialog.close();
    return homeComponent.createPromise;
  }

  private createAccount() {
    this.createPromise = this.accountService
      .simpleRegister(this.simpleRegister)
      .subscribe(
        result => {
          this.showLoading = false;
          this.loginService.setLoginData(result.data);
          let afterCreateUrl = this.loginService.getLoginRedirectUrl();
          if (!afterCreateUrl) {
            afterCreateUrl = "login";
          }
          this.loginService.setLoginRedirectUrl("");
          this.router.navigateByUrl(afterCreateUrl);
        },
        response => {
          this.showLoading = false;
          this.notificationService.info("Info", response.error);
        }
      );
    return this.createPromise;
  }

  public getErrorMessage(formField: any) {
    if (formField.hasError("required")) {
      return "Required field";
    } else if (formField.hasError("email")) {
      return "Not a valid email";
    } else if (formField.hasError("minlength")) {
      return (
        "Field must be at least " +
        formField.errors.minlength.requiredLength +
        " characters long."
      );
    } else if (formField.hasError("maxlength")) {
      return (
        "Field can be max " +
        formField.errors.maxlength.requiredLength +
        " characters long."
      );
    } else if (formField.hasError("pattern")) {
      return "Field must be only [0-9a-zA-Z-_.] characters";
    } else if (formField.hasError("emailRegistration")) {
      return "Email already registered.";
    } else if (formField.hasError("usernameRegistration")) {
      return "Username already registered.";
    }
  }
}
