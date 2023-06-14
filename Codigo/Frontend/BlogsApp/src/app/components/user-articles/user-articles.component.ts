import { Component, OnInit } from '@angular/core';
import { Article } from '../../models/article.model';
import { ArticleService } from '../../services/article.service';
import { Observer, catchError, of, take } from 'rxjs';
import { CommentsModalComponent } from '../comments-modal/comments-modal.component';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-articles',
  templateUrl: './user-articles.component.html',
  styleUrls: ['./user-articles.component.scss'],
})
export class UserArticlesComponent implements OnInit {
  articles: Article[] = [];
  errorMessage: string = '';
  userId = 0;
  userName = '';

  constructor(
    private userService: UserService,
    private route: ActivatedRoute // Inyectar ActivatedRoute
  ) {}

  ngOnInit() {
    this.userId = +this.route.snapshot.params['id']; // Obtener el id del usuario de los parámetros de la ruta
    this.getUser();
    this.getArticles();
  }

  getUser() {
    const observer: Observer<User> = {
      next: (user: User) => {
        this.userName = user.username;
      },
      error: (error: any) => {
        // Manejo de errores
        console.log('No se encontró el usuario', error);
      },
      complete: () => {
        // Acciones completadas (opcional)
      },
    };
    this.userService.getUserById(this.userId).subscribe(observer);
  }

  getArticles() {
    const observer: Observer<Article[]> = {
      next: (articles: Article[]) => {
        this.setArticles(articles);
        if (articles.length === 0) {
          this.errorMessage = 'No se encontraron artículos';
        } else {
          this.errorMessage = '';
        }
      },
      error: (error: any) => {
        // Manejo de errores
        console.log('No se encontraron articulos', error);
        this.setArticles([]);
        this.errorMessage = 'No se encontraron artículos';
      },
      complete: () => {
        // Acciones completadas (opcional)
      },
    };
    this.userService.getArticlesByUser(this.userId).subscribe(observer);
  }

  private setArticles = (articles: Article[] | undefined) => {
    if (!articles) this.articles = [];
    else this.articles = articles;
  };
}
