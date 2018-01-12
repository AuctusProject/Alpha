import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from "../../services/account.service";
import { NotificationsService } from "angular2-notifications";

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  constructor(private route: ActivatedRoute, 
              private accountService: AccountService,
              private notificationService: NotificationsService) { }

  ngOnInit() {
    this.confirmEmail();
  }

  confirmEmail() : void{
    let code = this.route.snapshot.queryParams['c'];
    this.accountService.confirmEmail(code).subscribe(
      response => {
        if (response){
          this.notificationService.success("Sucess", "Email confirmed!");
        }
      }
    );
  }

}
