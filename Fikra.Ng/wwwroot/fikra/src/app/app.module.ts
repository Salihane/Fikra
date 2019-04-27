import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatButtonModule,
  MatInputModule,
  MatCardModule,
  MatDatepickerModule,
  MatNativeDateModule
} from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TaskComponent } from './task/task.component';
import { TaskApiService } from './services/task.api.service';
import { DateFormatterService } from './services/date.formatter.service';

@NgModule({
  declarations: [
    AppComponent,
    TaskComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [
    TaskApiService,
    DateFormatterService,
    MatDatepickerModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
