import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { ArticleFormComponent } from './components/article-form/article-form.component';
import { AuthGuard } from './guards/auth.guard';
import { ArticleViewComponent } from './components/article-view/article-view.component';
import { UserFormComponent } from './components/user-form/user-form.component';
import { UserArticlesComponent } from './components/user-articles/user-articles.component';
import { ImporterComponent } from './components/importer/importer.component';
import { LogsComponent } from './components/logs/logs.component';
import { RankingComponent } from './components/ranking/ranking.component';
import { StatsComponent } from './components/stats/stats.component';
import { EditarUsuarioComponent } from './components/editar-usuario/editar-usuario.component';
import { EditAllUsersComponent } from './components/edit-all-users/edit-all-users.component';
import { OffensiveWordsComponent } from './components/offensive-words/offensive-words.component';
import { Content } from './models/content.model';
import { ContentToRevisionComponent } from './components/content-to-revision/content-to-revision.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'register', component: UserFormComponent },
  { path: 'user/:id', component: UserArticlesComponent },
  { path: 'add', component: ArticleFormComponent },
  { path: 'edit/:id', component: ArticleFormComponent },
  { path: 'importer', component: ImporterComponent },
  { path: 'articles/:id', component: ArticleViewComponent },
  { path: 'logs', component: LogsComponent },
  {path: 'ranking', component: RankingComponent},
  {path: 'stats', component: StatsComponent},
  { path: 'editUser', component: EditarUsuarioComponent},
  {path: 'editUsers', component: EditAllUsersComponent},
  {path: 'offensiveWords', component: OffensiveWordsComponent},
  {path: 'revision', component: ContentToRevisionComponent},
  { path: '**', redirectTo: '' }, // this line goes at the end
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
