import { Injectable } from '@angular/core';

@Injectable()
export class DateFormatterService {
  formatDate(date: Date): Date {
    let year = date.getFullYear();
    let month = date.getMonth() + 1;
    let day = date.getDate();

    return new Date(year, month, day);

    // let monthLeteral = this.padZero(month.toString());
    // let dayLeteral = this.padZero(day.toString());

    // return `${monthLeteral}-${dayLeteral}-${year}`;
  }

  private padZero(date: string) {
    if (date.length == 1) {
      return `0${date}`;
    }

    return date;
  }
}
