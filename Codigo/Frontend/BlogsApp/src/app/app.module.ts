import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginService } from './services/login.service';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { AuthService } from './services/auth.service';
import { ArticleService } from './services/article.service';
import { ArticleFilterPipe } from './components/home/pipes/article-filter.pipe';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    HomeComponent,
    ArticleFilterPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    LoginService,
    AuthService,
    ArticleService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
