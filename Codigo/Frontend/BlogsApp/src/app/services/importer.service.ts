import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IImporter } from '../models/importer.model';

@Injectable({
  providedIn: 'root'
})
export class ImporterService {
  apiUrl = 'http://localhost:5050/api/articles';

  constructor(private http: HttpClient) { }

  getImporter() {
    return this.http.get(this.apiUrl+ '/importers');
  }

  postImporter(importerName: string, path: string) {
    const body = { importerName, path };
    return this.http.post(this.apiUrl+ '/import', body);
  }
}
