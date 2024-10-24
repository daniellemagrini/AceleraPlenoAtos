import { PaComponent } from './Componentes/pa/pa.component';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './Componentes/navbar/navbar.component';
import { HomeComponent } from './Componentes/home/home.component';
import { LoginComponent } from './Componentes/login/login.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; 
import { Login } from './Models/login';
import { AuthenticationService } from './Services/authentication.service';
import { AuthenticationInterceptor } from './Services/interceptor';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { UsuarioComponent } from './Componentes/usuario/usuario.component';
import { ParametrizacaoComponent } from './Componentes/parametrizacao/parametrizacao.component';
import { IdleService } from './Services/idle.service';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [
      RouterOutlet, LoginComponent, NavbarComponent, HomeComponent, HttpClientModule, FormsModule,
       ReactiveFormsModule, ToastrModule, UsuarioComponent, ParametrizacaoComponent, PaComponent
      ],
    providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }],
})

export class AppComponent implements OnInit {
  title = 'Acelera';
  loginDto = new Login();

  constructor(private authService: AuthenticationService, private idleService: IdleService) {}

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.idleService.resetTimeout();
    }
  }
}
