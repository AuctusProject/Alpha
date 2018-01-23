import { Component, OnInit, Input } from '@angular/core';
import { AdvisorService } from "../../../services/advisor.service";
import { Advisor } from "../../../model/advisor/advisor";
import { Portfolio } from "../../../model/portfolio/portfolio";
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

  constructor(private advisorService: AdvisorService,
    private route: ActivatedRoute) { } 

  ngOnInit() {
    this.getPortfolios();
  }

  getPortfolios():void{
    this.advisorId = +this.route.snapshot.paramMap.get('id');
    this.advisorService.getAdvisorDetails(this.advisorId).subscribe(portfolios => this.portfolios = portfolios);
  }

}
