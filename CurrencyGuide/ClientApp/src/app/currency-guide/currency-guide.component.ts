import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Currency } from "./models/currency.model";
import { CurrencyGuideService } from "./currency-guide.service";
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-currency-guide',
  templateUrl: './currency-guide.component.html',
  styleUrls: ['./currency-guide.component.css']
})
export class CurrencyGuideComponent {
  currencies: Currency[];
  currencyNames = {};
  currencyFrom: number = 0;
  currencyTo: number = 0;
  countToChange: number = 1;
  conversionRate: number;
  date: any;
  panelOpenState = false;
  maxDate = new Date();

  get bothCurrenciesSet() {
    return this.currencyFrom && this.currencyTo;
  }

  constructor(private cgs: CurrencyGuideService, private _snackBar: MatSnackBar) {
    cgs.getCurrencies().subscribe(result => {
      this.currencies = result;
      result.map(currency => { this.currencyNames[currency.id] = currency.name });
    }, errorResult => this._snackBar.open(errorResult.error, null, { duration: 2000, }));
  }

  reload() {
    if (this.currencyFrom && this.currencyTo) {
      this.cgs.getConversionRate(this.currencyFrom, this.currencyTo, this.date ? this.date.format("YYYY-MM-DD") : '').subscribe(result => {
        this.conversionRate = result.rate;
        this.countToChange = 1;
      },
        errorResult => this._snackBar.open(errorResult.error, null, { duration: 2000, }));
    }
  }
  switchCurrencies() {
    var currency = this.currencyFrom;
    this.currencyFrom = this.currencyTo;
    this.currencyTo = currency;
    this.reload();
  }

  openPanel() {
    this.panelOpenState = true;
  }

  closePanel() {
    this.panelOpenState = false;
    this.date = null;
    this.reload();
  }
  
}
