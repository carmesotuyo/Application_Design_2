import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentBasic, CommentDto, CommentNotify } from '../models/comment.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'http://localhost:5050/api/comments';

  private offlineComments: CommentNotify[] = [];

  setOfflineComments(comments: CommentNotify[]): void {
    this.offlineComments = comments;
  }

  getOfflineComments(): CommentNotify[] {
    return this.offlineComments;
  }

  removeOfflineComment(commentId: number): void {
    this.offlineComments = this.offlineComments.filter(comment => comment.commentId !== commentId);
  }

  constructor(private http: HttpClient) { }

  postComment(comment: CommentBasic): Observable<CommentDto> {
    return this.http.post<CommentDto>(this.apiUrl, comment);
  }

  postReply(comment: CommentBasic, idParent: number): Observable<CommentDto> {
    const url = `${this.apiUrl}/${idParent}`;
    return this.http.post<CommentDto>(url, comment);
  }
}