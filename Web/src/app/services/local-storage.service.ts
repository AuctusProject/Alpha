import { Injectable } from '@angular/core';


@Injectable()
export class LocalStorageService {

  constructor() { }

  public setLocalStorage(key: string, value: any): void {
    if (window) window.localStorage.setItem(key, typeof value === "string" ? value : JSON.stringify(value));
  }

  public getLocalStorage(key: string): any {
    return window ? window.localStorage.getItem(key) : null;
  }

  public removeLocalStorage(key: string): void {
    if (window) window.localStorage.removeItem(key);
  }

}
