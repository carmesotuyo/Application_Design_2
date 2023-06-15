import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(
    private _authService: AuthService,
    private _router: Router,
  ) {}

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isAdmin = this._authService.isAuthorizedAdmin();
    if (!this._authService.isAuthenticated()) {
      this._router.navigateByUrl(''); // deberían redireccionar al login
      return false;
    }
    if (!isAdmin) {
      this._router.navigateByUrl('/home');
      return false;
    }
    return true;
  }
}
