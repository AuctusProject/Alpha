import { Component, OnInit, Input } from '@angular/core';
import { AdviceService } from "../../../services/advice.service";
import { Projections } from "../../../model/advice/projections";

@Component({
  selector: 'app-projection-tab',
  templateUrl: './projection-tab.component.html',
  styleUrls: ['./projection-tab.component.css']
})
export class ProjectionTabComponent implements OnInit {
  projections: Projections;

  constructor(private adviceService: AdviceService) {
    this.projections = new Projections();
  }

  ngOnInit() {
    this.adviceService.getProjections().subscribe(
      projections => {
        if (projections != undefined)
          this.projections = projections
      }
    )
  }
}
