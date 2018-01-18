
import { Injectable } from '@angular/core';

@Injectable()
export class StorageHelper {

    constructor() {
    }

    public getLoginToken(): string {
        return localStorage.getItem('loginToken');
    }

    public setLoginToken(loginToken: string) {
        localStorage.setItem('loginToken', loginToken);
    }

}
