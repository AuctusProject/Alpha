import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Asset } from '../../../../../model/asset/asset';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';

@Component({
  selector: 'portfolio-distribution-row',
  templateUrl: './portfolio-distribution-row.component.html',
  styleUrls: ['./portfolio-distribution-row.component.css']
})
export class PortfolioDistributionRowComponent implements OnInit {
  @Input() rowIndex: number;
  @Output() addNewRow = new EventEmitter<number>();

  assets: Asset[] = [ {
    id: 13,
    name: "Bitcoin",
    code: "BTC",
    type: 1
  }];
  productForm: FormControl = new FormControl();

  filteredAssets: Observable<Asset[]>;

  constructor() { }

  ngOnInit() {
    this.filteredAssets = this.productForm.valueChanges
      .pipe(
      startWith(''),
      map(val => this.filter(val))
      );
  }

  filter(val: string): Asset[] {
    return this.assets.filter(asset =>
      this.filterAsset(asset, val));
  }

  filterAsset(asset: Asset, val: string): boolean {
    return this.nameAndCode(asset).toLowerCase().indexOf(val.toLowerCase()) != -1;
  }

  nameAndCode(asset: Asset) {
    return asset.name + " (" + asset.code + ")";
  }

  public addNewRowClick() {
    this.addNewRow.emit(this.currentRow);
  }

  public removeRowClick() {
    this.removeRow.emit(this.currentRow);
  }
}
