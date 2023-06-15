import { Component, OnInit } from '@angular/core';
import { ArticleView } from '../../models/articleView.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../services/article.service';
import { CommentDto } from 'src/app/models/comment.model';
import { CommentService } from 'src/app/services/comment.service';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

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
  estado = 'Publico'
  estados = ['Publico', 'En revisión', 'Editado', 'Eliminado']
  
  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private commentService: CommentService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.getArticle();
  }

  getArticle(): void {
    const articleId = this.route.snapshot.params['id'];
    this.articleService.getArticle(articleId).subscribe((article) => {
      this.article = article;
      this.comments = article.commentsDtos! ? article.commentsDtos : [];
      this.estado = this.estados[article.state];
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

  eplyToComment(comment: CommentDto, replyText: string): void {
    if(replyText) {
      this.commentService
        .postReply(
          { body: replyText, articleId: this.article!.id },
          comment.id
        )
        .subscribe(() => {
          this.newComments[comment.id] = '';
          this.getArticle();
          this.toastr.success('Respuesta creada con éxito', 'Éxito');
        }, error => {
          this.toastr.error(error.error, 'Error');
        });
    } else {
      this.toastr.error('No se proporcionó texto para la respuesta', 'Error');
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
          this.toastr.success('Artículo eliminado', 'Éxito');
          this.router.navigateByUrl('/home');
        },
        (error) => {
          console.error('Error al eliminar el artículo', error);
          this.toastr.error(error.error, 'Error');
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
