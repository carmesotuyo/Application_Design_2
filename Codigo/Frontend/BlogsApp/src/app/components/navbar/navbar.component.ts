import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  username: string | null = '';

  constructor(private authService: AuthService, private loginService: LoginService) { }

  ngOnInit() {
    // Obtener el nombre de usuario del servicio de autenticación
    this.username = this.authService.getUsername();
  }

  logout() {
    const userId = '14'; // Reemplazar con el ID del usuario actual
    const token = this.authService.getToken(); // Obtener el token del almacenamiento local
  
    this.loginService.logout(userId, token).subscribe(
      () => {
        // Lógica adicional después del logout
        console.log('Logout correcto');
        this.authService.logout();
      },
      (error) => {
        // Manejo de errores
      }
    );
  }
}
