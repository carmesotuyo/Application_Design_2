import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, tap, throwError } from 'rxjs';
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
  
    return this.http.get<Article[]>(this.apiUrl, { headers, params }).pipe(
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






  getArticle(id: number, token: string): Observable<Article> {
    const url = `${this.apiUrl}/${id}`;
    const headers = new HttpHeaders().set('token', token);

    return this.http.get<Article>(url, { headers });
  }
}

