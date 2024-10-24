import { Routes } from '@angular/router';
import { LoginComponent } from './Componentes/login/login.component';
import { LayoutComponent } from './Componentes/layout/layout.component';
import { HomeComponent } from './Componentes/home/home.component';
import { authGuard } from './Guard/auth.guard';
import { UsuarioComponent } from './Componentes/usuario/usuario.component';
import { ParametrizacaoComponent } from './Componentes/parametrizacao/parametrizacao.component';
import { RedefinicaoSenhaComponent } from './Componentes/redefinicaoSenha/redefinicaoSenha.component';
import { guestGuard } from './Guard/guest.guard';
import { LogComponent } from './Componentes/log-processamento/log.component';
import { RoleGuardService } from './Guard/role.guard';
import { PaComponent } from './Componentes/pa/pa.component';
import { authTemporaryGuard } from './Guard/auth-temporary.guard';

export const routes: Routes = [
    {
        path: '', redirectTo: 'login', pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [guestGuard]
    },
    {
        path: 'redefinicaoSenha',
        component: RedefinicaoSenhaComponent,
        canActivate: [guestGuard, authTemporaryGuard] 
    },
    {
        path: '',
        component: LayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'home',
                component: HomeComponent
            },
            {
                path: 'usuario',
                component: UsuarioComponent,
                canActivate: [RoleGuardService]
            },
            {
                path: 'parametrizacao',
                component: ParametrizacaoComponent,
                canActivate: [RoleGuardService]
            },
            {
                path: 'log',
                component: LogComponent,
                canActivate: [RoleGuardService]
            },
            {
                path: 'pa',
                component: PaComponent,
            }
        ]
    }
];
