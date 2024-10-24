import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class IdleService {
  private timeoutId: any;
  private idleTimeout = 15 * 60 * 1000; // 15 minutos

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private ngZone: NgZone
  ) {
    if (this.isBrowser()) {
      this.resetTimeout();
      this.setupActivityListeners();
    }
  }

  private isBrowser(): boolean {
    return typeof window !== 'undefined';
  }

  private setupActivityListeners() {
    if (this.isBrowser()) {
      ['click', 'mousemove', 'keypress', 'scroll', 'touchstart'].forEach(event => {
        window.addEventListener(event, () => this.resetTimeout());
      });
    }
  }

  public resetTimeout() {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
    this.timeoutId = setTimeout(() => this.logout(), this.idleTimeout);
  }

  private logout() {
    this.ngZone.run(() => {
      this.authenticationService.logout();
      this.router.navigate(['/login']);
      alert('Você foi deslogado devido à inatividade.');
    });
  }
}
