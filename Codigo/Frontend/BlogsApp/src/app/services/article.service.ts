import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, Subject, catchError, tap, throwError } from 'rxjs';
import { Article } from '../models/article.model';
import { IDeleteResponse } from '../interfaces/delete-response-interface';
import { ArticleView } from '../models/articleView.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  private apiUrl = 'http://localhost:5050/api/articles'; // Reemplaza la URL con la ruta de tu API de artículos
  articleDeleted = new Subject<Article>();

  constructor(private http: HttpClient) { }

  getArticles(token: string, search?: string): Observable<Article[]> {
    let params = new HttpParams();
  
    if (search !== undefined && search !== null) {
      params = params.set('search', search);
    }
  
    return this.http.get<Article[]>(this.apiUrl, { params }).pipe(
      tap((articles: Article[]) => {
        // Verificar si la respuesta es una lista vacía
        if (articles.length === 0) {
          console.log('No se encontraron artículos');
          // Realizar acciones adicionales, como mostrar un mensaje en el componente
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          throwError(() => new Error('Artículos no encontrados'));
        } else {
          throwError(() => new Error('Ha ocurrido un error'));
        }
    
        return throwError(() => error);
      })
    );
  }

  getArticle(id: number): Observable<ArticleView> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<ArticleView>(url);
  }

  postArticle(article: Article): Observable<any> {
    return this.http.post<any>(this.apiUrl, article);
  }

  public putArticle(articleToUpdate: Article): Observable<Article> {
    const url = `${this.apiUrl}/${articleToUpdate.id}`;
    return this.http.put<Article>(`${url}`, articleToUpdate);
  }

  deleteArticle(articleId: number): Observable<any> {
    const url = `${this.apiUrl}/${articleId}`;
    return this.http.delete(url);
  }

}

