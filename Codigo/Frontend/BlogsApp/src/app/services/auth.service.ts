import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenSubject = new BehaviorSubject<string>('');
  public token$ = this.tokenSubject.asObservable();

  constructor() { }

  public setToken(token: string): void {
    this.tokenSubject.next(token);
  }

  public getToken(): string {
    return this.tokenSubject.value;
  }

  public isLoggedIn(): boolean {
    return !!this.tokenSubject.value;
  }

  public logout(): void {
    this.tokenSubject.next('');
    // También puedes realizar otras acciones al cerrar sesión, como limpiar el almacenamiento local
  }
}
