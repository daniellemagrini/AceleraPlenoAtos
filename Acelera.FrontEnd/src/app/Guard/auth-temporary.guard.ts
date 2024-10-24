import { CanActivateFn, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { inject } from '@angular/core';

export const authTemporaryGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => { 
  const router = inject(Router);
  const access = route.queryParams['access'];

  if (access === 'true') {
    sessionStorage.setItem('canAccessRedefinicaoSenha', 'true');
    return true;
  }

  const canAccess = sessionStorage.getItem('canAccessRedefinicaoSenha');

  if (canAccess === 'true') {
    sessionStorage.removeItem('canAccessRedefinicaoSenha');
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};
