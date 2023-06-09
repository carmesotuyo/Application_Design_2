import { Component, OnInit } from '@angular/core';
import { Article } from '../../models/article.model';
import { ArticleService } from '../../services/article.service';
import { Observer, catchError, of, take } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  articles: Article[] = [];
  searchKeyword: string = '';
  private token: string = '';
  errorMessage: string = '';

  constructor(
    private articleService: ArticleService,
  ) {}

  ngOnInit() {
    this.getArticles(this.token);
  }

  getArticles(token: string, search?: string) {
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
      }
    };
  
    this.articleService.getArticles(token, search).subscribe(observer);
  }

  loadArticles(): void {
    this.articleService.getArticles(this.token, this.searchKeyword).pipe(
      take(1),
      catchError((err) => {
        console.log({err});
        return of(err);
      }),
    )
    .subscribe(
      (articles: Article[]) => {
        this.setArticles(articles);
      },
      (error) => {
        console.error(error);
        // Manejo de errores
      }
    );
  }

  onSearch(): void {
    this.getArticles(this.token, this.searchKeyword);
  }

  private setArticles = (articles: Article[] | undefined) => {
    if(!articles) this.articles = [];
    else this.articles = articles;
  };
}

