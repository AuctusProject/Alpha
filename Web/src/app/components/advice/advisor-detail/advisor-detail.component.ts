import { Component, OnInit, Input } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { Advisor } from "../../../model/advice/advisor";
import { Portfolio } from "../../../model/advice/portfolio";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-advisor-detail',
  templateUrl: './advisor-detail.component.html',
  styleUrls: ['./advisor-detail.component.css']
})
export class AdvisorDetailComponent implements OnInit {

  @Input() advisor: Advisor;
  advisorId: number;
  portfolios: Portfolio[];

  constructor(private adviceService: AdviceService,
    private route: ActivatedRoute) { } 

  ngOnInit() {
    this.getPortfolios();
  }

  getPortfolios():void{
    this.advisorId = +this.route.snapshot.paramMap.get('id');
    this.adviceService.getAdvisorDetails(this.advisorId).subscribe(portfolios => this.portfolios = portfolios);
  }

}
