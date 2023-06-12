import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { Article } from '../models/article.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl = 'http://localhost:5050/api/users';
  constructor(private http: HttpClient) { }

  getArticlesByUser(userId: number): Observable<Article[]> {  
    const url = `${this.apiUrl}/${userId}/articles`;
    return this.http.get<Article[]>(url).pipe(
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

  public getRanking(dateFrom: string, dateTo: string, top: number): Observable<User[]> {
    const url = `${this.apiUrl}/ranking`;
    const params = new HttpParams()
      .set('dateFrom', dateFrom)
      .set('dateTo', dateTo)
      .set('top', top.toString());

    return this.http.get<User[]>(url, { params });
  }

  public postUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  public updateUser(user: User): Observable<User> {
    //const id = user.id;
    const id = 0;
    const url = `${this.apiUrl}/${id}`;
    return this.http.patch<User>(url, user);
  }

  public deleteUser(userId: number): Observable<any> {
    const url = `${this.apiUrl}/${userId}`;
    return this.http.delete<any>(`${url}`);
  }
}
