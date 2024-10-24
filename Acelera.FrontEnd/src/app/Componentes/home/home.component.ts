import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { UserService } from '../../Services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  perfis: string[] = [];
  username: string = '';
  unidade: string = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    const login = sessionStorage.getItem('login');
    if (login) {
      this.userService.getUserProfiles(login).subscribe({
        next: (perfis) => this.perfis = perfis,
        error: (err) => console.error('Erro ao buscar perfis do usuário:', err)
      });

      this.userService.getUserByID(login).subscribe({
        next: (user) => {
          if (user && user.value) {
            this.username = user.value.descnomeusuario;
          }
        },
        error: (err) => console.error('Erro ao buscar o nome do usuário:', err)
      });

      this.userService.getUnidadeByLogin(login).subscribe({
        next: (response) => {
          this.unidade = response.unidade; 
        },
        error: (err) => console.error('Erro ao buscar unidade do usuário:', err)
      });
    }
  }

  hasAccess(profileDescriptions: string[]): boolean {
    return this.perfis.some(profile => profileDescriptions.includes(profile));
  }
}
