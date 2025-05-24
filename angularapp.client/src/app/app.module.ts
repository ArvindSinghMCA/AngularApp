import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component'; // standalone: true inside this file
import { PositionsComponent } from './components/positions/positions.component'; // standalone: true

@NgModule({
  imports: [               
    BrowserModule,
    HttpClientModule,
    AppComponent,
    PositionsComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
