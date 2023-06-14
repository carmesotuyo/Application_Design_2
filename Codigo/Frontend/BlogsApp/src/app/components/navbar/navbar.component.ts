import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  username: string | null = '';

  constructor(private authService: AuthService, private loginService: LoginService, private router: Router) { }

  ngOnInit() {
    // Obtener el nombre de usuario del servicio de autenticación
    this.username = this.authService.getUsername();
  }

  logout() {  
    this.loginService.logout().subscribe({
      next: () => {
        this.authService.logout();
        this.router.navigateByUrl('/login');
      },
      error: (error) => {
        // Manejo de errores
      }
    });
  }

  toMyArticles() {
    this.router.navigateByUrl('/user/' + this.authService.getUserId());
  }
}
