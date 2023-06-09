import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenSubject = new BehaviorSubject<string>('');
  public token$ = this.tokenSubject.asObservable();
  private _userRoleKey = 'userRole';
  private _userName = 'userName';
  private _userTokenKey = 'userToken';

  constructor() { }

  public setToken(token: string): void {
    //this.tokenSubject.next(token);
    sessionStorage.setItem(this._userTokenKey, token);
  }

  public getToken(): string | null {
    //return this.tokenSubject.value;
    return sessionStorage.getItem(this._userTokenKey);
  }

  public removeToken(): void {
    //this.tokenSubject.next('');
    sessionStorage.removeItem(this._userTokenKey);
  }

  public isLoggedIn(): boolean {
    return !!this.getToken();
  }

  public logout(): void {
    //this.tokenSubject.next('');
    this.removeToken();
    // También puedes realizar otras acciones al cerrar sesión, como limpiar el almacenamiento local
  }

  public getUsername(): string | null {
    return sessionStorage.getItem(this._userName);
  }

  public setUsername(username: string): void {
    sessionStorage.setItem(this._userName, username);
  }

  public getUserRole(): string | null {
    return sessionStorage.getItem(this._userRoleKey);
  }

  public setUserRole(userRole: string): void {
    sessionStorage.setItem(this._userRoleKey, userRole);
  }

  public removeUserRole(): void {
    sessionStorage.removeItem(this._userRoleKey);
  }

  public isAuthenticated(): boolean {
    const token = this.getToken();
    if(!token) {
      return false;
    }
    return true;
  }

  public isAuthorized(role: string): boolean {
    const userRole = this.getUserRole();
    if(!userRole || userRole?.toLowerCase() !== role?.toLowerCase()) {
      return false;
    }
    return true;
  }
}
