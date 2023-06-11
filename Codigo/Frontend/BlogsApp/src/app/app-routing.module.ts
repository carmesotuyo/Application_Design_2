import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import{ArticleFormComponent} from './components/article-form/article-form.component';
import { AuthGuard } from './guards/auth.guard';
import { ArticleViewComponent } from './components/article-view/article-view.component';

const routes: Routes = [
  { path: '', component: LoginComponent},
  { path: 'login', component: LoginComponent},
  {path: 'home', component: HomeComponent, canActivate: [AuthGuard],},
  {path: 'add', component: ArticleFormComponent},
  { path: 'articles/:id', component: ArticleViewComponent },
  { path: '**', redirectTo: '', }, // this line goes at the end
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
