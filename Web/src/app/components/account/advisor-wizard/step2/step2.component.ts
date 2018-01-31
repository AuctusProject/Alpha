import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor'
import { RiskType } from '../../../../model/account/riskType'
import { Asset } from '../../../../model/asset/asset'
import { PublicService } from '../../../../services/public.service'

@Component({
  selector: 'advisor-step2',
  templateUrl: './step2.component.html',
  styleUrls: ['./step2.component.css']
})
export class Step2Component implements OnInit {
  portfolioRisks: RiskType[] =
  [
    { value: 1, description: "Conservative"},
    { value: 2, description: "Moderately Conservative" },
    { value: 3, description: "Moderately Aggressive" },
    { value: 4, description: "Aggressive" },
    { value: 5, description: "Very Aggressive"}
  ]; 

  @Input() model: Advisor;
  @Output() onStepFinished = new EventEmitter<Advisor>();

  assets: Asset[];
  constructor(private publicService: PublicService) { }

  ngOnInit() {
    this.publicService.listAssets().subscribe(assets => this.assets = assets);
  }

  public back() {
    this.onStepFinished.emit(null);
  }

  public next() {
    this.onStepFinished.emit(this.model);
  }

  onSubmit() {
    this.next();
  }
}
