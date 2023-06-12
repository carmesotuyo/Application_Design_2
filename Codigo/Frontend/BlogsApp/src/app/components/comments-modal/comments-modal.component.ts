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

  ngOnInit(): void {}

  replyToComment(comment: CommentNotify, replyText: string): void {
    if(replyText) {
      this.commentService
        .postReply(
          { body: replyText, articleId: comment.articleId },
          comment.commentId
        )
        .subscribe(() => {
          // Filtrar comentario respondido
          this.data.comments = this.data.comments.filter((c: CommentNotify) => c.commentId !== comment.commentId);
          // Actualizar comentarios offline
          this.commentService.removeOfflineComment(comment.commentId);
        });
      alert('Replying to comment ' + comment.commentId + '.');
    } else {
      alert('Debe ingresar un texto para responder al comentario ' + comment.commentId);
    }
  }
}
