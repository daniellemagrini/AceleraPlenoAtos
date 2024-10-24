import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthenticationService } from './../Services/authentication.service';
import { UserService } from './../Services/user.service';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {
  constructor(
    private authService: AuthenticationService,
    private userService: UserService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    const userLogin = this.authService.getLoggedUserId();
    if (!userLogin) {
      this.router.navigate(['/login']);
      return of(false);
    }

    return this.userService.getUserProfiles(userLogin).pipe(
      map(userProfiles => {
        const hasAccess = userProfiles.includes('Administrador') || userProfiles.includes('TI'); // 1 - Administrador e 2 - TI
        
        if (hasAccess) {
          return true;
        } else {
          this.router.navigate(['/home']);
          return false;
        }
      }),
      catchError(() => {
        this.router.navigate(['/home']);
        return of(false);
      })
    );
  }
}
