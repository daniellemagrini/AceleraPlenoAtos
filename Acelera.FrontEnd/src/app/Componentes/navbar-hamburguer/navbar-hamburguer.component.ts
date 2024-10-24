import { Component } from '@angular/core';
import { MenuHamburguerComponent } from '../menu-hamburguer/menu-hamburguer.component';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-navbar-hamburguer',
  standalone: true,
  imports: [MenuHamburguerComponent, MatSidenavModule, MatToolbarModule, CommonModule],
  templateUrl: './navbar-hamburguer.component.html',
  styleUrl: './navbar-hamburguer.component.css'
})
export class NavbarHamburguerComponent {
  opened = true;
}
