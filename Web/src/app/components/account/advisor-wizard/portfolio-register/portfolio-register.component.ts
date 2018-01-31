import { Component, OnInit, Input } from '@angular/core';
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

  public onAssetDistributionChanged(assetDistribution: AssetDistribution, assetRow: number) {
    this.assetsDistributionRows[assetRow] = assetDistribution;
  }

}
