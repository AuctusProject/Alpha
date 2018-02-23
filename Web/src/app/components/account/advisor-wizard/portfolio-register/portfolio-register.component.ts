import { Component, OnInit, Input, Output, ViewChild, ChangeDetectorRef, ApplicationRef, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Asset } from '../../../../model/asset/asset';
import { RiskType } from '../../../../model/account/riskType'
import { AssetDistribution } from '../../../../model/asset/assetDistribution'
import { PortfolioRequest } from '../../../../model/portfolio/portfolioRequest'
import { DistributionRequest } from '../../../../model/portfolio/distributionRequest'
import { PortfolioService } from '../../../../services/portfolio.service'
import { FormControl, Validators, FormGroup } from '@angular/forms';


@Component({
  selector: 'portfolio-register',
  templateUrl: './portfolio-register.component.html',
  styleUrls: ['./portfolio-register.component.css']
})
export class PortfolioRegisterComponent implements OnInit {
  @Input() index: number;
  @Input() assets: Asset[];
  assetsDistributionRows: AssetDistribution[];
  @Input() model: PortfolioRequest;
  @Output() onPortfolioSaved = new EventEmitter();
  @ViewChild('portfolioRegisterForm') portfolioRegisterForm;
  totalDistributionPercentage = 0;
  totalPercentageForm: FormControl = new FormControl("", [Validators.required, Validators.min(100), Validators.max(100)]);

  constructor(private ref: ChangeDetectorRef, private portfolioService: PortfolioService) { }

  ngOnInit() {

    if (this.model == null) {
      this.model = new PortfolioRequest();
    }
    this.assetsDistributionRows = [];
    this.assetsDistributionRows.push(new AssetDistribution());
    this.assetsDistributionRows[0].percentage = 100;
    this.portfolioRegisterForm.control.addControl("TotalPercentage", this.totalPercentageForm);
  }

  public editPortfolio() {
    this.model.isEditing = true;
  }

  public addNewRow(rowIndex: number) {
    this.assetsDistributionRows.push(new AssetDistribution());
  }

  public removeRow(rowIndex: number) {
    if (rowIndex > -1) {
      this.assetsDistributionRows.splice(rowIndex, 1);
    }
    this.calculateTotalDistributionPercentage();
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
    this.ref.detectChanges();
  }

  public currentAvailableAssets(currentAssetDistributionRow: AssetDistribution): Asset[] {
    var availableAssets: Asset[] = [];
    for (let asset of this.assets) {
      var isAssetInUse = false;
      var isAssetFromCurrentRow = asset.id == currentAssetDistributionRow.id;
      if (!isAssetFromCurrentRow) {
        for (let assetDistribution of this.assetsDistributionRows) {
          if (asset.id == assetDistribution.id) {
            isAssetInUse = true;
          }
        }
      }
      if (!isAssetInUse || isAssetFromCurrentRow) {
        availableAssets.push(asset);
      }
    }
    return availableAssets;
  }

  addFormControls(rowNumber, event) {
    this.portfolioRegisterForm.control.addControl("Product[" + rowNumber + "]", event.productForm);
    this.portfolioRegisterForm.control.addControl("Percentage[" + rowNumber + "]", event.percentageForm);
  }

  removeFormControls(rowNumber) {
    this.portfolioRegisterForm.control.removeControl("Product[" + rowNumber + "]");
    this.portfolioRegisterForm.control.removeControl("Percentage[" + rowNumber + "]");
  }

  anyEmptyRow() {
    for (let assetDistribution of this.assetsDistributionRows) {
      if (!assetDistribution || !assetDistribution.code)
        return true;
    }
    return false;
  }


  onSubmit() {
    for (let row of this.assetsDistributionRows) {
      if (!this.model.distribution.some(item => item.assetId === row.id)) {
        this.model.distribution.push({
          assetId: row.id,
          percentage: row.percentage
        });
      }
    }

    if (this.model.id == null) {
      this.portfolioService.createPortfolio(this.model)
        .subscribe(model => {
          this.model.id = model.id;
          this.model.isEditing = false;
          if (this.onPortfolioSaved) {
            this.onPortfolioSaved.emit();
          }
        });
    } else {
      this.portfolioService.updatePortfolio(this.model)
        .subscribe(model => {
          this.model.isEditing = false;
          if (this.onPortfolioSaved) {
            this.onPortfolioSaved.emit();
          }
        });
    }
  }
}
