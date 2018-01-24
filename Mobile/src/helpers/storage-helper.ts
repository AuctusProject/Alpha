
import { Injectable } from '@angular/core';

@Injectable()
export class StorageHelper {

    constructor() {
    }

    public getLoginToken(): any {
        return localStorage.getItem('loginToken');
    }

    public setLoginToken(loginToken: any) {
        localStorage.setItem('loginToken', loginToken);
    }

}
