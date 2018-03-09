import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AdvisorService } from "../../../services/advisor.service"
import { Advisor } from "../../../model/advisor/advisor"
import { constants } from "../../../util/contants";

@Component({
  selector: 'app-advisor-details',
  templateUrl: './advisor-details.component.html',
  styleUrls: ['./advisor-details.component.css']
})
export class AdvisorDetailsComponent implements OnInit {
  advisor: Advisor;

  maximumNumberOfPortfoliosPerAdvisor : number = constants.maximumNumberOfPortfoliosPerAdvisor;

  constructor(private route: ActivatedRoute, private advisorService: AdvisorService) { }

  ngOnInit() {
    this.route.params.subscribe(params => 
      this.advisorService.getAdvisor(params['id']).subscribe(advisor => this.advisor = advisor)
    )
  }
}
