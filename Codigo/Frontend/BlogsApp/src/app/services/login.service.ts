import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login_URL, Home_URL } from '../utils/routes'
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = `${environment.API_HOST_URL}/api/sessions`;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post<any>(this.apiUrl, body);
  }

  logout(): Observable<any> {
    const url = `${this.apiUrl}`;
    return this.http.patch<any>(url, null);
  }
}
