import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter',
})

@Injectable()
export class FilterPipe implements PipeTransform {
    transform(items: any[], field: string, value: string): any[] {
        if (value) {
            return items.filter(it => it[field].toLowerCase().indexOf(value.toLowerCase()) > -1);
        }
        return items;
    }
}
