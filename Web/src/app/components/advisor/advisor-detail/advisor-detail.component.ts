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

  constructor(private advisorService: AdvisorService,
    private route: ActivatedRoute) { } 

  ngOnInit() {
    this.getDetails();
  }

  getDetails():void{
    this.advisorId = +this.route.snapshot.paramMap.get('id');
    this.advisorService.getAdvisorDetails(this.advisorId).subscribe(advisor => this.advisor = advisor);
  }

}
