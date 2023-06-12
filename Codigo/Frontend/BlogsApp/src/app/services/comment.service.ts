import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentBasic, CommentDto } from '../models/comment.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'http://localhost:5050/api/comments';

  constructor(private http: HttpClient) { }

  postComment(comment: CommentBasic): Observable<CommentDto> {
    return this.http.post<CommentDto>(this.apiUrl, comment);
  }

  postReply(comment: CommentBasic, idParent: number): Observable<CommentDto> {
    const url = `${this.apiUrl}/${idParent}`;
    return this.http.post<CommentDto>(url, comment);
  }
}
