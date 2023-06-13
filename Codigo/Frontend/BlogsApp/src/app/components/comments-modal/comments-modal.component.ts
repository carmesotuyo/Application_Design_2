import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentNotify } from 'src/app/models/comment.model';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-comments-modal',
  templateUrl: './comments-modal.component.html',
  styleUrls: ['./comments-modal.component.scss']
})
export class CommentsModalComponent implements OnInit {
  replies: { [key: number]: string } = {};
  newComments: CommentNotify[] = [] ;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private commentService: CommentService
  ) {}

  ngOnInit(): void {
    this.data.comments.forEach((comment: CommentNotify) => {
      comment.reply = '';
    });
  }

  replyToComment(comment: CommentNotify): void {
    if(comment.reply) {
      this.commentService
        .postReply(
          { body: comment.reply, articleId: comment.articleId },
          comment.commentId
        )
        .subscribe(() => {
          // Filtrar comentario respondido
          this.data.comments = this.data.comments.filter((c: CommentNotify) => c.commentId !== comment.commentId);
          // Actualizar comentarios offline
          this.commentService.removeOfflineComment(comment.commentId);
        });
      alert('Comentario enviado correctamente');
    } else {
      alert('Debe ingresar un texto para responder al comentario ' + comment.commentId);
    }
  }
}
