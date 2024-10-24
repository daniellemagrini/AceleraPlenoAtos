import { AuthenticationService } from './authentication.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http'; 
import { Observable, catchError, throwError, switchMap } from 'rxjs';  
import { environment } from '../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userProfileUrl = `${environment.apiUrl}/Perfil/GetUserProfiles`;
  private getAllGrupoAcessoUrl = `${environment.apiUrl}/Perfil/GetAllGrupoAcesso`;
  private getAllUnidadesUrl = `${environment.apiUrl}/Unidade/GetAllUnidades`;
  private getUnidadeByLoginUrl = `${environment.apiUrl}/Unidade/GetUnidadeByLogin`;
  private cadastrarUsuarioUrl = `${environment.apiUrl}/User/CadastroUsuario`; 
  private getUserByNomeUrl = `${environment.apiUrl}/User/GetUserByNome`;
  private getUserByIDUrl = `${environment.apiUrl}/User/GetUserByID`;
  private getUserByEmailUrl = `${environment.apiUrl}/User/GetUserByEmail`;
  private updateUsuarioUrl = `${environment.apiUrl}/User/UpdateUsuario`;
  private inativarUsuarioUrl = `${environment.apiUrl}/User/InativarUsuario`;
  private verificaLoginInativoUrl = `${environment.apiUrl}/Auth/VerificaLoginInativo`;

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  public getUserProfiles(login: string): Observable<string[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<string[]>(`${this.userProfileUrl}/${login}`, { headers });
}

  public getPerfilByID(id: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${environment.apiUrl}/Perfil/GetPerfilByID?id=${id}`, { headers });
  }

  public getAllGrupoAcesso(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(this.getAllGrupoAcessoUrl, { headers });
  }

  public getAllUnidades(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any[]>(this.getAllUnidadesUrl, { headers });
  }

  public getUnidadeByLogin(login: string): Observable<{ unidade: string }> {
    const headers = this.getAuthHeaders();
    return this.http.get<{ unidade: string }>(`${this.getUnidadeByLoginUrl}/${login}`, { headers });
  }

  public cadastrarUsuario(payload: any): Observable<any> {
    const headers = this.getAuthHeaders();
    const userLogin = this.authenticationService.getLoggedUserId(); 

    if (!userLogin) {
      return throwError(() => new Error('Usuário não está logado.'));
    }

    return this.authenticationService.GetUsersByLogin(userLogin).pipe(
      switchMap(usuarioLogado => {
        payload.loginCriador = usuarioLogado.idusuariosistema;

        let params = new HttpParams().set('loginCriador', payload.loginCriador.toString());
        payload.listaGrupoAcesso.forEach((perfil: any) => {
          params = params.append('listaGrupoAcesso', perfil.toString());
        });

        return this.http.post<any>(this.cadastrarUsuarioUrl, payload.user, { headers, params });
      }),
      catchError(this.handleError)
    );
  }

  public getUserByNome(nome: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.getUserByNomeUrl}?nome=${nome}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }
  
  public getUserByID(id: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.getUserByIDUrl}?id=${id}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }
  
  public getUserByEmail(email: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.getUserByEmailUrl}?email=${email}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  public updateUsuario(payload: any): Observable<any> {
    const headers = this.getAuthHeaders();
    const userLogin = this.authenticationService.getLoggedUserId(); 

    if (!userLogin) {
        return throwError(() => new Error('Usuário não está logado.'));
    }

    return this.authenticationService.GetUsersByLogin(userLogin).pipe(
        switchMap(usuarioLogado => {
            payload.loginAlteradoPor = usuarioLogado.idusuariosistema;

            let params = new HttpParams().set('loginAlteradoPor', payload.loginAlteradoPor.toString());

            console.log("Payload para atualização de usuário:", payload);
            console.log("URL construída:", this.updateUsuarioUrl, params.toString());

            return this.http.put<any>(this.updateUsuarioUrl, payload.user, { headers, params });
        }),
        catchError(this.handleError)
    );
  }

  public inativarUsuario(payload: any): Observable<any> {
    const headers = this.getAuthHeaders();
    const userLogin = this.authenticationService.getLoggedUserId(); 

    if (!userLogin) {
        return throwError(() => new Error('Usuário não está logado.'));
    }

    return this.authenticationService.GetUsersByLogin(userLogin).pipe(
        switchMap(usuarioLogado => {
            payload.loginInativadoPor = usuarioLogado.idusuariosistema;

            let params = new HttpParams().set('loginInativadoPor', payload.loginInativadoPor.toString());

            return this.http.put<any>(this.inativarUsuarioUrl, payload.user, { headers, params });
        }),
        catchError(this.handleError)
    );
  }

  public VerificaLoginInativo(login: string): Observable<boolean> {
    const headers = this.getAuthHeaders();
    return this.http.get<boolean>(`${this.verificaLoginInativoUrl}?login=${login}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Erro desconhecido!';
    if (error.error instanceof ErrorEvent) {
      // Erro no lado do cliente
      errorMessage = `Erro: ${error.error.message}`;
    } else {
      // Erro no lado do servidor
      if (error.status === 404) {
        errorMessage = 'Usuário não encontrado!';
      } else if (error.status === 400 && error.error.message) {
        errorMessage = error.error.message;
      } else if (error.status === 500 && error.error.message) {
        errorMessage = error.error.message;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}
