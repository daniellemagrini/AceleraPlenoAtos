import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav'; 
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuService } from '../../Services/menu.service'
import { Router } from '@angular/router';


@Component({
  selector: 'app-menu-hamburguer',
  standalone: true,
  imports: [MatSidenavModule, RouterModule, CommonModule],
  templateUrl: './menu-hamburguer.component.html',
  styleUrl: './menu-hamburguer.component.css'
})

export class MenuHamburguerComponent {

  menuOpen = false;

  constructor(private menuService: MenuService, private router: Router) {
    this.menuService.menuOpen$.subscribe(open => {
      this.menuOpen = open;
    });
  }

  toggleMenu() {
    this.menuService.toggleMenu();
  }

  logout() {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
