import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import{HttpClientModule, HTTP_INTERCEPTORS} from  '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';

import { HomeModule } from './home/home.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { NgxSpinner, NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './core/interceptors/LoadingInterceptor';
import { JwtInterceptor } from './core/interceptors/JWT.interceptor';
import { CampainComponent } from './campain/campain.component';
import { CommentsComponent } from './comments/comments.component';

@NgModule({
  declarations: [
    AppComponent,
    CampainComponent,
    CommentsComponent,
 
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CoreModule,
    HomeModule,
    NgxSpinnerModule
  ],
  providers: [

    {provide: HTTP_INTERCEPTORS,useClass: ErrorInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS,useClass: LoadingInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS,useClass: JwtInterceptor, multi:true},

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
