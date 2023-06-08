import { Component, OnInit } from '@angular/core';
import { Article } from '../../models/article.model';
import { ArticleService } from '../../services/article.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  articles: Article[] = [];

  constructor(
    private articleService: ArticleService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const token = this.authService.getToken() || '';
    this.getArticles(token);
  }

  getArticles(token: string, search?: string) {
    this.articleService.getArticles(token, search).subscribe(
      (articles: Article[]) => {
        this.articles = articles;
        console.log(this.articles);
      },
      (error) => {
        // Manejo de errores
      }
    );
  }
}

