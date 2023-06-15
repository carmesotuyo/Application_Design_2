import { Component } from '@angular/core';
import { NotificationService } from 'src/app/services/notification.service';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';

@Component({
  selector: 'app-nav-bar-admin',
  templateUrl: './nav-bar-admin.component.html',
  styleUrls: ['./nav-bar-admin.component.scss']
})
export class NavBarAdminComponent {
  constructor(private offensivewordsService: OffensivewordsService, private notificationService: NotificationService) { }

  hayNotificaciones: boolean = this.notificationService.hayNotificaciones;

  notificationDismisser(){
    this.offensivewordsService.notificationDismissed().subscribe((response: any) => {
      // Procesa la respuesta aquí
      this.hayNotificaciones = false;
      this.notificationService.setHayNotificaciones(false);
      console.log(response);
    });
  }
}
