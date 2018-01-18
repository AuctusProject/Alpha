import { Component, OnInit } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { Projections } from "../../../model/advice/projections";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  projections: Projections;

  constructor(private adviceService: AdviceService) {
    this.projections = new Projections();
  }

  ngOnInit() {
    this.adviceService.getProjections().subscribe(
      projections => {
        if (projections!=undefined)
          this.projections = projections
      }
    )
  }
}
