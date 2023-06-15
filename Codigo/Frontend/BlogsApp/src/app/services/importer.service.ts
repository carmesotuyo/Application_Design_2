import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IImporter } from '../models/importer.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImporterService {
  private apiUrl = environment.API_HOST_URL + '/api/articles';

  constructor(private http: HttpClient) { }

  getImporter() {
    return this.http.get(this.apiUrl+ '/importers');
  }

  postImporter(importerName: string, path: string) {
    const body = { importerName, path };
    return this.http.post(this.apiUrl+ '/import', body);
  }
}
