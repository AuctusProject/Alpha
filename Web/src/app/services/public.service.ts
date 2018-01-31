import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpService } from './http.service';
import { Asset } from "../model/asset/asset";

@Injectable()
export class PublicService {
  private listAssetsUrl = this.httpService.apiUrl("public/v1/assets");

  constructor(private httpService: HttpService) { }

  listAssets(): Observable<Asset[]> {
    return this.httpService.get(this.listAssetsUrl);
  }

}
