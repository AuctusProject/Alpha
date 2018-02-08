import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'
import { RiskType } from '../../../../model/account/riskType'
import { Asset } from '../../../../model/asset/asset'
import { PublicService } from '../../../../services/public.service'

@Component({
  selector: 'portfolio-step',
  templateUrl: './portfolio-step.component.html',
  styleUrls: ['./portfolio-step.component.css']
})
export class PortfolioStepComponent implements OnInit {

  @Input() advisorModel: Advisor;
  @Output() onBackStep = new EventEmitter<Advisor>();
  @Output() onNextStep = new EventEmitter<Advisor>();

  portfolioRisks: RiskType[] =
  [
    { value: 1, description: "Conservative"},
    { value: 2, description: "Moderately Conservative" },
    { value: 3, description: "Moderately Aggressive" },
    { value: 4, description: "Aggressive" },
    { value: 5, description: "Very Aggressive"}
  ]; 

  assets: Asset[];
  constructor(private publicService: PublicService) { }

  ngOnInit() {
    this.publicService.listAssets().subscribe(assets => this.assets = assets);
  }

  public back() {
    this.onBackStep.emit();
  }

  public next() {
    this.onNextStep.emit(this.advisorModel);
  }
}
