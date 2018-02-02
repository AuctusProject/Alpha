import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { Asset } from '../../../../../model/asset/asset';
import { FormControl, Validators, FormGroup, ValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';
import { AssetDistribution } from '../../../../../model/asset/assetDistribution'

@Component({
  selector: 'portfolio-distribution-row',
  templateUrl: './portfolio-distribution-row.component.html',
  styleUrls: ['./portfolio-distribution-row.component.css']
})
export class PortfolioDistributionRowComponent implements OnChanges, OnInit {
  @Input() rowNumber: number;
  @Input() disableRemoveButton: boolean;
  @Input() showAddButton: boolean;
  @Input() assetDistribution: AssetDistribution;
  @Output() addNewRow = new EventEmitter();
  @Output() removeRow = new EventEmitter();
  @Output() assetDistributionChanged = new EventEmitter<AssetDistribution>();
  @Input() availableAssets: Asset[];
  @Input() formGroup: FormGroup;
  @Output() onAddFormControls = new EventEmitter<any>();
  @Output() onRemoveFormControls = new EventEmitter();


  selectedAssets: Asset[];


  productForm: FormControl = new FormControl('Asset', this.selectedValidAssetValidator);
  percentageForm: FormControl = new FormControl("Percentage["+this.rowNumber+"]", [Validators.required, this.greaterThanZero, Validators.max(100)]);

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

    this.addFormControls(this.rowNumber);
  }

  greaterThanZero(control: FormControl) {
    let value = control.value;
    if (value > 0)
      return null;
    return {
      zeroOrLess: true
    };
  }



  selectedValidAssetValidator(control: FormControl) {
    let asset = control.value;
    if (!asset && !asset.code) {
      return {
        assetNotSelected: true
      };
    }
    return null;
  }

  addFormControls(rowNumber) {
    this.formGroup.addControl("Product[" + rowNumber + "]", this.productForm);
    this.formGroup.addControl("Percentage[" + rowNumber + "]", this.percentageForm);
  }

  removeFormControls(rowNumber) {
    this.formGroup.removeControl("Product[" + rowNumber + "]");
    this.formGroup.removeControl("Percentage[" + rowNumber + "]");
  }

  ngOnChanges(changes: SimpleChanges) {
    const rowNumber: SimpleChange = changes.rowNumber;
    if (rowNumber && rowNumber.previousValue && rowNumber.currentValue && rowNumber.currentValue != rowNumber.previousValue) {
      this.removeFormControls(rowNumber.previousValue);
      this.addFormControls(rowNumber.currentValue);
    }
    const availableAssets: SimpleChange = changes.availableAssets;
    if (availableAssets && availableAssets.currentValue && availableAssets.previousValue && availableAssets.currentValue.length != availableAssets.previousValue.length) {
      this.filteredAssets = this.productForm.valueChanges
        .pipe(
        startWith(''),
        map(val => this.filter(val))
        );
    }
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

  filter(val: any): Asset[] {
    return this.availableAssets.filter(asset =>
      this.filterAsset(asset, val));
  }

  filterAsset(asset: Asset, val: any): boolean {
    if (val != null && val.name != null)
      val = val.name;
    return this.nameAndCode(asset).toLowerCase().indexOf(val.toLowerCase()) != -1;
  }

  nameAndCode(asset: Asset) {
    return (asset != null && asset.code) ? asset.code + " - " + asset.name + "" : "";
  }

  public addNewRowClick() {
    this.addNewRow.emit();
  }

  public removeRowClick() {
    this.removeFormControls(this.rowNumber);
    this.removeRow.emit();
  }

  onAssetInputBlur() {
    if (this.productForm.value && !this.productForm.value.code)
      this.productForm.setValue("");
  }
}
