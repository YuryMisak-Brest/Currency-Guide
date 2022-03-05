import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CurrencyGuideComponent } from './currency-guide/currency-guide.component';
import { CurrencyGuideService } from "./currency-guide/currency-guide.service";
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { MatExpansionModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule } from '@angular/material' 
import { MatMomentDateModule } from "@angular/material-moment-adapter";
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CurrencyGuideComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
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
    FormsModule,
    RouterModule.forRoot([
      { path: 'about', component: HomeComponent, pathMatch: 'full' },
      { path: '', component: CurrencyGuideComponent }
    ])
  ],
  providers: [CurrencyGuideService, MatMomentDateModule, MatDatepickerModule],
  bootstrap: [AppComponent]
})
export class AppModule { }
