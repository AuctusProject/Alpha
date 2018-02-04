import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Asset } from '../../../../../model/asset/asset';
import { FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';
import { AssetDistribution } from '../../../../../model/asset/assetDistribution'

@Component({
  selector: 'portfolio-distribution-row',
  templateUrl: './portfolio-distribution-row.component.html',
  styleUrls: ['./portfolio-distribution-row.component.css']
})
export class PortfolioDistributionRowComponent implements OnInit {
  @Input() disableRemoveButton: boolean;
  @Input() showAddButton: boolean;
  @Input() assetDistribution: AssetDistribution;
  @Output() addNewRow = new EventEmitter();
  @Output() removeRow = new EventEmitter();
  @Output() assetDistributionChanged = new EventEmitter<AssetDistribution>();
  @Input() availableAssets: Asset[];

  productForm: FormControl = new FormControl();
  percentageForm: FormControl = new FormControl();

  filteredAssets: Observable<Asset[]>;

  constructor() { }

  ngOnInit() {
    this.filteredAssets = this.productForm.valueChanges
      .pipe(
      startWith(''),
      map(val => this.filter(val))
    );

    this.productForm.valueChanges.subscribe(val => this.assetChange(val));
    this.percentageForm.valueChanges.subscribe(val => this.percentageChange(val));
  }

  assetChange(val: any) {
    this.fillAssetDistributionWithAssetInformation(val);
    this.emitAssetDistributionChanged();
  }

  fillAssetDistributionWithAssetInformation(val: any) {
    if (val != null) {
      this.assetDistribution.id = val.id;
      this.assetDistribution.name = val.name;
      this.assetDistribution.code = val.code;
      this.assetDistribution.type = val.type;
    }
    else {
      this.assetDistribution.id = null;
      this.assetDistribution.name = null;
      this.assetDistribution.code = null;
      this.assetDistribution.type = null;
    }
  }

  percentageChange(val: any) {
    this.emitAssetDistributionChanged();
  }

  emitAssetDistributionChanged() {
    this.assetDistributionChanged.emit(this.assetDistribution);
  }

  filter(val: string): Asset[] {
    return this.availableAssets.filter(asset =>
      this.filterAsset(asset, val));
  }

  filterAsset(asset: Asset, val: any): boolean {
    if (val != null && val.name != null)
      val = val.name;
    return this.nameAndCode(asset).toLowerCase().indexOf(val.toLowerCase()) != -1;
  }

  nameAndCode(asset: Asset) {
    return asset != null ? asset.name + " (" + asset.code + ")" : "";
  }

  public addNewRowClick() {
    this.addNewRow.emit();
  }

  public removeRowClick() {
    this.removeRow.emit();
  }
}
