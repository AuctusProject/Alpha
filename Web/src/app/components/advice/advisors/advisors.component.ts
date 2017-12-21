import { Component, OnInit } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { Advisor } from "../../../model/advice/advisor";


@Component({
  selector: 'app-advisors',
  templateUrl: './advisors.component.html',
  styleUrls: ['./advisors.component.css']
})
export class AdvisorsComponent implements OnInit {
  advisors: Advisor[];
  selectedAdvisor: Advisor;

  constructor(private adviceService: AdviceService) { }

  ngOnInit() {
    this.adviceService.getAdvisors().subscribe(
      advisors => {
        this.advisors = advisors
      }
    )
  }  

  onSelect(advisor: Advisor): void {
    this.selectedAdvisor = advisor;
  }
}
