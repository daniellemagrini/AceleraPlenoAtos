import { CanActivateFn } from '@angular/router';
import { Router } from '@angular/router';
import { AuthenticationService } from './../Services/authentication.service';
import { inject } from '@angular/core';

export const guestGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    router.navigate(['/home']);
    return false;
  } else {
    return true;
  }
};
