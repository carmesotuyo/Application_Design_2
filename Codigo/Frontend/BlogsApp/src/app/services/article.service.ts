import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from '../models/article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  private apiUrl = 'http://localhost:5050/api/articles'; // Reemplaza la URL con la ruta de tu API de artículos

  constructor(private http: HttpClient) { }

  getArticles(token: string, search?: string): Observable<Article[]> {
    const headers = new HttpHeaders().set('token', token);
    let params = new HttpParams();

    if (search !== undefined && search !== null) {
      params = params.set('search', search);
    }

    return this.http.get<Article[]>(this.apiUrl, { headers, params });
  }

  getArticle(id: number, token: string): Observable<Article> {
    const url = `${this.apiUrl}/${id}`;
    const headers = new HttpHeaders().set('token', token);

    return this.http.get<Article>(url, { headers });
  }
}

