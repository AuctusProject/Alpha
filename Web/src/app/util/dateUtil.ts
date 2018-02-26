export class DateUtil {

    public static DiffDays(date1, date2) : number {
        var oneDay = 24 * 60 * 60 * 1000;
        var diffDays = Math.round(Math.abs((date1.getTime() - date2.getTime()) / (oneDay)));
        return diffDays;
    }
}