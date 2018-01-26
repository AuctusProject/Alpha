import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.css']
})
export class ActionBarComponent implements OnInit {
  @Input() tabs: any; 
  @Input() user: string = null;
  @Output() onTabSelected = new EventEmitter<number>();
  public selectedTab = 0;

  constructor(private loginService: LoginService) { }

  ngOnInit() {
    this.user = this.loginService.getUser();
  }



  logout(): void {
    this.loginService.logout();
  }

  public tabClick(index: number) {
    this.selectedTab = index;
    this.onTabSelected.emit(index);
  }
}
