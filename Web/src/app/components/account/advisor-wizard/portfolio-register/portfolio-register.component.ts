import { Component, OnInit, Input, ViewChild, ChangeDetectorRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Asset } from '../../../../model/asset/asset';
import { RiskType } from '../../../../model/account/riskType'
import { AssetDistribution } from '../../../../model/asset/assetDistribution'
import { PortfolioRequest } from '../../../../model/portfolio/portfolioRequest'
import { PublicService } from '../../../../services/public.service'
import { FormControl, Validators, FormGroup } from '@angular/forms';

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
  totalDistributionPercentage = 0;
  totalPercentageForm: FormControl = new FormControl("", [Validators.required, Validators.min(100), Validators.max(100)]);

  constructor(private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.model = new PortfolioRequest();
    this.assetsDistributionRows = [];
    this.assetsDistributionRows.push(new AssetDistribution());
    this.assetsDistributionRows[0].percentage = 100;
    this.portfolioRegisterForm.control.addControl("TotalPercentage", this.totalPercentageForm);
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
      this.calculateTotalDistributionPercentage();
    }
  }

  public onAssetDistributionChanged(assetDistribution: AssetDistribution, assetRow: number) {
    this.assetsDistributionRows[assetRow] = assetDistribution;
    this.calculateTotalDistributionPercentage();
  }

  calculateTotalDistributionPercentage() {
    var total = 0;
    for (let distribution of this.assetsDistributionRows) {
      if (distribution.percentage)
        total += distribution.percentage;
    }
    this.totalDistributionPercentage = total;
    //if (this.totalDistributionPercentage > 100) {
    //  this.totalPercentageForm.setErrors({ "greater": this.totalDistributionPercentage > 100 });
    //}
    //else if (this.totalDistributionPercentage < 100) {
    //  this.totalPercentageForm.setErrors({ "lower": this.totalDistributionPercentage < 100 });
    //}
    //else {
    //  this.totalPercentageForm.setErrors(null);
    //}
    this.ref.detectChanges();
  }

  public findInvalidControls() {
    const invalid = [];
    const controls = this.portfolioRegisterForm.controls;
    for (const name in controls) {
      if (controls[name].invalid) {
        invalid.push(name);
      }
    }
    return invalid;
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
