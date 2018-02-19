import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advisor } from '../../../../model/advisor/advisor';
import { RiskType } from '../../../../model/account/riskType';
import { Asset } from '../../../../model/asset/asset';
import { PublicService } from '../../../../services/public.service';
import { PortfolioRequest } from '../../../../model/portfolio/portfolioRequest';

@Component({
  selector: 'portfolio-step',
  templateUrl: './portfolio-step.component.html',
  styleUrls: ['./portfolio-step.component.css']
})
export class PortfolioStepComponent implements OnInit {

  @Input() advisorModel: Advisor;
  @Input() portfolioList: Array<PortfolioRequest>;
  @Output() onBackStep = new EventEmitter<Advisor>();
  @Output() onNextStep = new EventEmitter<Advisor>();

  public assets: Asset[];
  

  constructor(private publicService: PublicService) { }

  ngOnInit() {
    this.publicService.listAssets().subscribe(assets => this.assets = assets);
    if(this.portfolioList.length === 0){
      this.addPortfolio();
    }
  }
  
  public back() {
    this.onBackStep.emit();
  }

  public next() {
    this.onNextStep.emit();
  }

  public addPortfolio() {
    let portfolio = new PortfolioRequest();
    portfolio.advisorId = this.advisorModel.id;
    portfolio.isEditing = true;
    this.portfolioList.push(portfolio);
  }

  public hasEditingPortfolio(): boolean {
    return this.portfolioList.some(item => item.isEditing);
  }
}
