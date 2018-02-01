import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Asset } from '../../../../model/asset/asset';
import { RiskType } from '../../../../model/account/riskType'
import { AssetDistribution } from '../../../../model/asset/assetDistribution'
import { PortfolioRequest } from '../../../../model/portfolio/portfolioRequest'
import { PublicService } from '../../../../services/public.service'


@Component({
  selector: 'portfolio-register',
  templateUrl: './portfolio-register.component.html',
  styleUrls: ['./portfolio-register.component.css']
})
export class PortfolioRegisterComponent implements OnInit {
  riskDescription = "Conservative";
  @Input() risk: RiskType;
  @Input() assets: Asset[];
  assetsDistributionRows: AssetDistribution[];
  @Input() model: PortfolioRequest;
  @ViewChild('portfolioRegisterForm') portfolioRegisterForm;
  isEditing = false;

  constructor() { }

  ngOnInit() {
    this.model = new PortfolioRequest();
    this.assetsDistributionRows = [];
    this.assetsDistributionRows.push(new AssetDistribution());
    this.assetsDistributionRows[0].percentage = 100;
  }

  public addPortfolio() {
    this.isEditing = true;
  }

  public addNewRow(rowIndex: number) {
    this.assetsDistributionRows.push(new AssetDistribution());
  }

  public removeRow(rowIndex: number) {
    if (rowIndex > -1) {
      this.assetsDistributionRows.splice(rowIndex, 1);
    }
  }

  public onAssetDistributionChanged(assetDistribution: AssetDistribution, assetRow: number) {
    this.assetsDistributionRows[assetRow] = assetDistribution;
  }

  onSubmit() {
  }


  addFormControls(rowNumber, event) {
    this.portfolioRegisterForm.control.addControl("Product[" + rowNumber + "]", event.productForm);
    this.portfolioRegisterForm.control.addControl("Percentage[" + rowNumber + "]", event.percentageForm);
  }

  removeFormControls(rowNumber) {
    this.portfolioRegisterForm.control.removeControl("Product[" + rowNumber + "]");
    this.portfolioRegisterForm.control.removeControl("Percentage[" + rowNumber + "]");
  }
}
