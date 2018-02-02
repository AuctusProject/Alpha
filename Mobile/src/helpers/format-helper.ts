
import { Injectable } from '@angular/core';

@Injectable()
export class FormatHelper {

    constructor() {
    }

    public static formatCurrency(valueToFormat: any, currency: string): string {
        let value = Number(valueToFormat);
        if (value >= 1000000) {
            return currency + (value / 1000000).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'M'
        } else if (value > 1000) {
            return currency + (value / 1000).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'k'
        } else {
            return currency + value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        }
    }
}
