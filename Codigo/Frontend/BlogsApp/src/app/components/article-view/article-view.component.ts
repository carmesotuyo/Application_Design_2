import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
  comments: CommentDto[] = [];
  newComment: string = '';
  showOptions = false;
  isOwner = false;
  userName = '';
  newComments: { [key: string]: string } = {};
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private commentService: CommentService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getArticle();
  }

  getArticle(): void {
    const articleId = this.route.snapshot.params['id'];
    this.articleService.getArticle(articleId).subscribe((article) => {
      this.article = article;
      this.comments = article.commentsDtos! ? article.commentsDtos : [];
      this.checkOwnership();
    });
  }

  checkOwnership(): void {
    this.userName = this.article!.username;
    const userLogged = this.authService.getUsername();
    const userOwner = this.userName;
    if (userLogged === userOwner) {
      this.isOwner = true;
    }
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

  replyToComment(comment: CommentDto, replyText: string): void {
    if(replyText) {
      this.commentService
        .postReply(
          { body: replyText, articleId: this.article!.id },
          comment.id
        )
        .subscribe(() => {
          this.newComments[comment.id] = '';
          this.getArticle();
        });
      // Código para mostrar una notificación en lugar de alert
      alert('Respuesta creada con éxito');
    } else {
      // Código para mostrar una notificación en lugar de alert
      alert('Parace que hubo un error al crear la respuesta');
    }
  }

  addComment(): void {
    this.commentService
      .postComment({ body: this.newComment, articleId: this.article!.id })
      .subscribe(() => {
        this.newComment = '';
        this.getArticle();
      });
  }

  deleteArticle(): void {
    if (this.article) {
      const articleToDelete = this.article;
      this.articleService.deleteArticle(this.article.id).subscribe(
        () => {
          this.articleService.articleDeleted.next(articleToDelete);
          alert('Artículo eliminado');
          this.router.navigateByUrl('/home');
        },
        (error) => {
          console.error('Error al eliminar el artículo', error);
          // Manejar el error de eliminación
        }
      );
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
