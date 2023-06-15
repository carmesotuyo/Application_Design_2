import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LogsService {
  private apiUrl = environment.API_HOST_URL + '/api/logs';
  constructor(private http: HttpClient) { }

  getLogs(from: string, to: string): Observable<any> {
    let params = new HttpParams();
    params = params.set('from', from);
    params = params.set('to', to);
    return this.http.get<any>(this.apiUrl, { params });
  }
}
