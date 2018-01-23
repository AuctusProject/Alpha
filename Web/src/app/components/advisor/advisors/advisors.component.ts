import { Component, OnInit } from '@angular/core';
import { AdvisorService } from "../../../services/advisor.service";
import { Advisor } from "../../../model/advisor/advisor";


@Component({
  selector: 'app-advisors',
  templateUrl: './advisors.component.html',
  styleUrls: ['./advisors.component.css']
})
export class AdvisorsComponent implements OnInit {
  advisors: Advisor[];
  selectedAdvisor: Advisor;

  constructor(private advisorService: AdvisorService) { }

  ngOnInit() {
    this.advisorService.getAdvisors().subscribe(
      advisors => {
        this.advisors = advisors
      }
    )
  }  

  onSelect(advisor: Advisor): void {
    this.selectedAdvisor = advisor;
  }
}
