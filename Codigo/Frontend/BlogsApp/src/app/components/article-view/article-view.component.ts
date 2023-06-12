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
  newComments: { [key: string]: string } = {};
  
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

  // replyToComment(comment: CommentDto, replyText: string): void {
  //   // Code to handle replying to a comment
  //   if(replyText) { // se verifica que replyText no sea null o cadena vacía
  //     this.commentService
  //       .postReply(
  //         { body: replyText, articleId: this.article!.id },
  //         comment.id
  //       )
  //       .subscribe((comment) => {
  //         //this.comments!.push(comment);
  //         this.newComments[comment.id] = ''; // se limpia el comentario una vez que ha sido enviado
  //       });
  //     alert('Replying to comment ' + comment.id + ' de ' + comment.user.username);
  //   } else {
  //     alert('Debe ingresar un texto para responder al comentario ' + comment.id);
  //   }
  // }

  replyToComment(comment: CommentDto, replyText: string): void {
    if(replyText) {
      this.commentService
        .postReply(
          { body: replyText, articleId: this.article!.id },
          comment.id
        )
        .subscribe((newComment) => {
          comment.subComments.push(newComment);
          this.newComments[comment.id] = '';
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
      .subscribe((newComment) => {
        this.comments!.push(newComment);
        this.newComment = '';
      });
  }

  // deleteArticle(): void {
  //   if (this.article) {
  //     const articleToDelete = this.article;
  //     this.articleService.deleteArticle(this.article.id).subscribe(() => {
  //       this.articleService.articleDeleted.next(articleToDelete);
  //       alert('Articulo eliminado');
  //       this.router.navigateByUrl('/home');
  //     });
  //   }
  // }

  // deleteArticle(): void {
  //   if (this.article) {
  //     const articleToDelete = this.article;
  //     this.articleService.deleteArticle(this.article.id).subscribe(
  //       () => {
  //         this.articleService.articleDeleted.next(articleToDelete);
  //         alert('Artículo eliminado');
  //         this.router.navigateByUrl('/home');
  //       },
  //       (error) => {
  //         console.error('Error al eliminar el artículo', error);
  //         // Manejar el error de eliminación
  //       }
  //     );
  //   }
  // }

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
