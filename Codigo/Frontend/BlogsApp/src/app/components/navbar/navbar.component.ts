import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { interval } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';
import { NotificationService } from 'src/app/services/notification.service';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  username: string | null = '';
  hayNotificaciones!: boolean;
  estaLogueado = this.authService.isAuthenticated();
  esBlogger = this.authService.isAuthorizedBlogger();
  esAdmin = this.authService.isAuthorizedAdmin();
  esModerador = this.authService.isAuthorizedMod();
  prevNotificaciones = false;
  private notifSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private loginService: LoginService,
    private router: Router,
    private offensivewordsService: OffensivewordsService,
    private notificationService: NotificationService,
    private toastr: ToastrService,
  ) {}

  ngOnInit() {
    this.username = this.authService.getUsername();
    this.getNotification();

    if(this.esAdmin || this.esModerador) {
      this.notifSubscription = interval(2000) // Genera un evento cada 10 segundos
      .pipe(switchMap(() => this.offensivewordsService.notificationViewer()))
      .subscribe((response: any) => {
        if (response) {
          this.hayNotificaciones = true;
          this.notificationService.setHayNotificaciones(true);
          if (!this.prevNotificaciones) { // Si prevNotificaciones era false
            this.toastr.info("Tienes nuevo contenido para revisar");
          }
        } else {
          this.hayNotificaciones = false;
          this.notificationService.setHayNotificaciones(false);
        }
        this.prevNotificaciones = this.hayNotificaciones;
      });
    }
  }

  ngOnDestroy() {
    this.notifSubscription?.unsubscribe();
  }

  getNotification() {
    this.offensivewordsService
      .notificationViewer()
      .subscribe((response: any) => {
        if (response) {
          this.hayNotificaciones = true;
        } else {
          this.hayNotificaciones = false;
        }
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
