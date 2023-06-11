import { Component, OnInit } from '@angular/core';
import { ArticleView   } from '../../models/articleView.model';
import { ActivatedRoute } from '@angular/router';
import { ArticleService } from '../../services/article.service';
import { CommentDto } from 'src/app/models/comment.model';

@Component({
  selector: 'app-article-view',
  templateUrl: './article-view.component.html',
  styleUrls: ['./article-view.component.scss']
})
export class ArticleViewComponent implements OnInit {
  article: ArticleView | null = null;
  comments: CommentDto[] | null = [];

  constructor(private route: ActivatedRoute, private articleService: ArticleService) { }

  ngOnInit(): void {
    const articleId = this.route.snapshot.params['id'];
    this.articleService.getArticle(articleId).subscribe((article) => {
      this.article = article;
      this.comments = article.commentsDtos;
    });
  }

  getFirstImage(): string {
    if (this.article?.image) {
      const images = this.article.image.split(' ');
      return images[0];
    }
    return '';
  }

  getSecondImage(): string {
    if (this.article?.image) {
      const images = this.article.image.split(' ');
      return images[1];
    }
    return '';
  }
}
