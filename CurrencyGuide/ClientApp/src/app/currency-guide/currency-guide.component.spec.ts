import { async, fakeAsync, tick, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencyGuideComponent } from './currency-guide.component';
import { BrowserModule, By } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { MatExpansionModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule } from '@angular/material'
import { MatMomentDateModule } from "@angular/material-moment-adapter";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CurrencyGuideService } from "./currency-guide.service";
import { InjectionToken } from '@angular/core';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

describe('CurrencyGuideComponent', () => {
  let fixture: ComponentFixture<CurrencyGuideComponent>;
  let currencies = [
    { "id": 1, "iso": "AED", "name": "United Arab Emirates Dirham" },
    { "id": 2, "iso": "NAD", "name": "Namibian Dollar" },
    { "id": 3, "iso": "NGN", "name": "Nigerian Naira" }
  ];
  let currencyNames = { 1: "United Arab Emirates Dirham",
    2: "Namibian Dollar",
    3: "Nigerian Nair"};

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        MatDatepickerModule,
        MatInputModule,
        MatMomentDateModule,
        MatExpansionModule,
        MatSnackBarModule,
        ReactiveFormsModule,
        MatAutocompleteModule,
        BrowserAnimationsModule,
        MatFormFieldModule,
        FormsModule],
      declarations: [CurrencyGuideComponent],
      providers: [CurrencyGuideService, { provide: 'BASE_URL', useValue: 'http://localhost' }]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrencyGuideComponent);
    fixture.detectChanges();
  });

  it('should display a loading', async(() => {
    const titleText = fixture.nativeElement.querySelector('em').textContent;
    expect(titleText).toEqual('Loading...');
  }));

  it('should change select value', (() => {
    fixture.componentInstance.currencies = currencies;
    fixture.detectChanges();
    const select: HTMLSelectElement = fixture.debugElement.query(By.css('.currency-from')).nativeElement;
      select.options.selectedIndex = select.options[2].index; // <-- select a new value
      select.dispatchEvent(new Event('change'));
      fixture.detectChanges();
    expect(select.options.selectedIndex).toEqual(select.options[2].index);
  }));

  it('should show result after both selected', (() => {
    fixture.componentInstance.currencies = currencies;
    fixture.componentInstance.currencyNames = currencyNames;
    fixture.componentInstance.conversionRate = 2;
    fixture.detectChanges();
    const selectFrom: HTMLSelectElement = fixture.debugElement.query(By.css('.currency-from')).nativeElement;
    const selectTo: HTMLSelectElement = fixture.debugElement.query(By.css('.currency-to')).nativeElement;
    selectFrom.options.selectedIndex = selectFrom.options[2].index; 
    selectFrom.dispatchEvent(new Event('change'));
    selectTo.options.selectedIndex = selectTo.options[1].index;
    selectTo.dispatchEvent(new Event('change'));
    fixture.detectChanges();
    const resultDiv = fixture.debugElement.query(By.css('.result-text'));
    expect(resultDiv.nativeElement.innerText).toEqual(' Nigerian Nair can be changed to 2.00 Namibian Dollar');
  }));

  it('should calculate result after both selected and number entered', (() => {
    fixture.componentInstance.currencies = currencies;
    fixture.componentInstance.currencyNames = currencyNames;
    fixture.componentInstance.conversionRate = 2;
    fixture.detectChanges();
    const selectFrom: HTMLSelectElement = fixture.debugElement.query(By.css('.currency-from')).nativeElement;
    const selectTo: HTMLSelectElement = fixture.debugElement.query(By.css('.currency-to')).nativeElement;
    selectFrom.options.selectedIndex = selectFrom.options[2].index;
    selectFrom.dispatchEvent(new Event('change'));
    selectTo.options.selectedIndex = selectTo.options[1].index;
    selectTo.dispatchEvent(new Event('change'));
    fixture.detectChanges();
    const resultDiv = fixture.debugElement.query(By.css('.result-text'));
    const input: HTMLInputElement = fixture.debugElement.query(By.css('.count-input')).nativeElement;
    input.value = "2";
    input.dispatchEvent(new Event('change'));
    fixture.detectChanges();
    expect(resultDiv.nativeElement.innerText).toEqual(' Nigerian Nair can be changed to 4.00 Namibian Dollar');
  }));
});
