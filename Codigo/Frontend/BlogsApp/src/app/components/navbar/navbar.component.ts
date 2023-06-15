import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { interval } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  username: string | null = '';
  hayNotificaciones!: boolean;

  constructor(
    private authService: AuthService,
    private loginService: LoginService,
    private router: Router,
    private offensivewordsService: OffensivewordsService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.username = this.authService.getUsername();
    this.getNotification();
    interval(2000) // Genera un evento cada 10 segundos
      .pipe(switchMap(() => this.offensivewordsService.notificationViewer()))
      .subscribe((response: any) => {
        if (response) {
          this.hayNotificaciones = true;
          this.notificationService.setHayNotificaciones(true);
        } else {
          this.hayNotificaciones = false;
          this.notificationService.setHayNotificaciones(false);
        }
        console.log(response);
      });
  }

  getNotification() {
    this.offensivewordsService
      .notificationViewer()
      .subscribe((response: any) => {
        // Procesa la respuesta aquí
        if (response) {
          this.hayNotificaciones = true;
        } else {
          this.hayNotificaciones = false;
        }
        console.log(response);
      });
  }

  logout() {
    this.loginService.logout().subscribe({
      next: () => {
        this.authService.logout();
        this.router.navigateByUrl('/login');
      },
      error: (error) => {
        // Manejo de errores
      },
    });
  }

  toMyArticles() {
    this.router.navigateByUrl('/user/' + this.authService.getUserId());
  }
}
