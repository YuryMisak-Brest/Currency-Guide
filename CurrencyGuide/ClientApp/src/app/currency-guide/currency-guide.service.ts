import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs"
import { Injectable, Inject } from '@angular/core';
import { Currency } from "./models/currency.model";
import { ConversionRate } from "./models/conversion-rate.model";

@Injectable()
export class CurrencyGuideService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }


  getCurrencies(): Observable<Currency[]> {
    return this.http.get<Currency[]>(this.baseUrl + 'api/currencies');
  }

  getConversionRate(currencyFrom: number, currencyTo: number, date: Date): Observable<ConversionRate> {
    return this.http.get<ConversionRate>(this.baseUrl + 'api/conversionRates?currencyFrom=' + currencyFrom + '&currencyTo=' + currencyTo + '&date=' + date);
  }

}
