import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';

@Component({
  selector: 'app-nav-bar-admin',
  templateUrl: './nav-bar-admin.component.html',
  styleUrls: ['./nav-bar-admin.component.scss']
})
export class NavBarAdminComponent {
  esBlogger = this.authService.isAuthorizedBlogger();
  esAdmin = this.authService.isAuthorizedAdmin();
  esModerador = this.authService.isAuthorizedMod();

  constructor(private offensivewordsService: OffensivewordsService, private notificationService: NotificationService, private authService: AuthService) { }

  hayNotificaciones: boolean = this.notificationService.hayNotificaciones;

  notificationDismisser(){
    this.offensivewordsService.notificationDismissed().subscribe((response: any) => {
      // Procesa la respuesta aquí
      this.hayNotificaciones = false;
      this.notificationService.setHayNotificaciones(false);
    });
  }
}
