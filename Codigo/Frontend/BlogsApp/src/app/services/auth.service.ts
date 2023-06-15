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
  private _userId = 'userId';
  private _bloggerKey = 'blogger';
  private _adminKey = 'admin';
  private _moderatorKey = 'moderator';


  constructor() { }

  public setToken(token: string): void {
    sessionStorage.setItem(this._userTokenKey, token);
  }

  public getToken(): string | null {
    return sessionStorage.getItem(this._userTokenKey);
  }

  public removeToken(): void {
    sessionStorage.removeItem(this._userTokenKey);
  }

  public isLoggedIn(): boolean {
    return !!this.getToken();
  }

  public logout(): void {
    this.removeToken();
    this.removeUserId();
  }

  public getUserId(): string | null {
    return sessionStorage.getItem(this._userId);
  }

  public setUserId(userId: string): void {
    sessionStorage.setItem(this._userId, userId);
  }

  public removeUserId(): void {
    sessionStorage.removeItem(this._userId);
  }

  public getUsername(): string | null {
    return sessionStorage.getItem(this._userName);
  }

  public setUsername(username: string): void {
    sessionStorage.setItem(this._userName, username);
  }



  


  public setBlogger(blogger: boolean): void {
    sessionStorage.setItem(this._bloggerKey, String(blogger));
  }

  public getBlogger(): boolean {
    const blogger = sessionStorage.getItem(this._bloggerKey);
    return blogger === 'true';
  }

  public setAdmin(admin: boolean): void {
    sessionStorage.setItem(this._adminKey, String(admin));
  }

  public getAdmin(): boolean {
    const admin = sessionStorage.getItem(this._adminKey);
    return admin === 'true';
  }

  public setModerator(moderator: boolean): void {
    sessionStorage.setItem(this._moderatorKey, String(moderator));
  }

  public getModerator(): boolean {
    const moderator = sessionStorage.getItem(this._moderatorKey);
    return moderator === 'true';
  }



  public isAuthenticated(): boolean {
    const token = this.getToken();
    if(!token) {
      return false;
    }
    return true;
  }

  public isAuthorizedMod(): boolean {
    return this.getAdmin() || this.getModerator();
  }
  
  public isAuthorizedAdmin(): boolean {
    return this.getAdmin();
  }
  
  public isAuthorizedBlogger(): boolean {
    return this.getBlogger();
  }
}
