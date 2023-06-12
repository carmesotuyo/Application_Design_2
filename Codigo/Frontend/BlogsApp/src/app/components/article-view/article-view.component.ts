import { Component, OnInit } from '@angular/core';
import { ArticleView } from '../../models/articleView.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../services/article.service';
import { CommentDto } from 'src/app/models/comment.model';
import { CommentService } from 'src/app/services/comment.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-article-view',
  templateUrl: './article-view.component.html',
  styleUrls: ['./article-view.component.scss'],
})
export class ArticleViewComponent implements OnInit {
  article: ArticleView | null = null;
  comments: CommentDto[] | null = [];
  newComment: string = '';
  showOptions = false;
  isOwner = false;
  userName = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private commentService: CommentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const articleId = this.route.snapshot.params['id'];
    this.articleService.getArticle(articleId).subscribe((article) => {
      this.article = article;
      this.comments = article.commentsDtos;
      this.userName = this.article.username;
      const userLogged = this.authService.getUsername();
      const userOwner = this.userName;
      if (userLogged === userOwner) {
        this.isOwner = true;
      }
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

  toggleCommentOptions(): void {
    //comment.showOptions = !comment.showOptions;
    this.showOptions = !this.showOptions;
  }

  replyToComment(comment: CommentDto): void {
    // Code to handle replying to a comment
    this.commentService
      .postReply(
        { body: this.newComment, articleId: this.article!.id },
        comment.id
      )
      .subscribe((comment) => {
        //this.comments!.push(comment);
        this.newComment = '';
      });
    alert('Replying to comment ' + comment.id + 'de ' + comment.user.username);
  }

  addComment(): void {
    this.commentService
      .postComment({ body: this.newComment, articleId: this.article!.id })
      .subscribe((comment) => {
        //this.comments!.push(comment);
        this.newComment = '';
      });
  }

  deleteArticle(): void {
    if (this.article) {
      const articleToDelete = this.article;
      this.articleService.deleteArticle(this.article.id).subscribe(() => {
        this.articleService.articleDeleted.next(articleToDelete);
        alert('Articulo eliminado');
        this.router.navigateByUrl('/home');
      });
    }
  }

  editArticle(): void {
    if (this.article) {
      this.router.navigateByUrl('/edit/' + this.article.id);
    }
  }

  goBack(): void {
    this.router.navigateByUrl('/home');
  }
}
