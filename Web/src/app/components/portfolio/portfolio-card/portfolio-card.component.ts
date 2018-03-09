import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Portfolio } from "../../../model/portfolio/portfolio";
import { Goal } from '../../../model/account/goal';
import { LocalStorageService } from "../../../services/local-storage.service";

@Component({
  selector: 'portfolio-card',
  templateUrl: './portfolio-card.component.html',
  styleUrls: ['./portfolio-card.component.css']
})

export class PortfolioCardComponent implements OnInit {
  @Input() portfolio: Portfolio;
  @Input() goal?: Goal;
  @Input() position: number;

  public readonly roboAdvisorType: number = 1;

  constructor(private router: Router, private localStorageService: LocalStorageService) { }

  ngOnInit() {
  }

  onDetailsClick() {
    if (this.goal) {
      this.localStorageService.setLocalStorage("currentGoal", this.goal);
    }
    this.router.navigateByUrl("/portfolio/" + this.portfolio.id);
  }
}

