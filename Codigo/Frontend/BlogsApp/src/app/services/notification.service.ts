import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  hayNotificaciones: boolean = false;
  constructor() { }

  public setHayNotificaciones(hayNotificaciones: boolean): void {
    this.hayNotificaciones = hayNotificaciones;
  }

  public getHayNotificaciones(): boolean {
    return this.hayNotificaciones;
  }
}
