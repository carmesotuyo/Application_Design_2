import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentBasic } from '../models/comment.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'http://localhost:5050/api/comments';

  constructor(private http: HttpClient) { }

  postComment(comment: CommentBasic): Observable<any> {
    return this.http.post<any>(this.apiUrl, comment);
  }

  postReply(comment: CommentBasic, idParent: number): Observable<any> {
    const url = `${this.apiUrl}/${idParent}`;
    return this.http.post<any>(url, comment);
  }
}
