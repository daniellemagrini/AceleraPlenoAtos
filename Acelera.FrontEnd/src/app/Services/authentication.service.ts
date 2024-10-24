import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import { Observable } from 'rxjs';  
import { environment } from '../../environments/environment'; 
import { Login } from '../Models/login';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private loginUrl = `${environment.apiUrl}/Auth/Login`;
  private atualizarSenhaUrl = `${environment.apiUrl}/Auth/AtualizarSenha`;
  private verificarCodAutUrl = `${environment.apiUrl}/Auth/VerificaCodAut`;
  private forgotPasswordUrl = `${environment.apiUrl}/Auth/EsqueciMinhaSenhaMail`;
  private GetUsersLoginByIDUrl = `${environment.apiUrl}/Auth/GetUsersLoginByID`; 
  private GetUsersByLoginUrl = `${environment.apiUrl}/Auth/GetUsersByLogin`;

  constructor(private http: HttpClient, private router: Router) { }

  private getAuthHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  public login(user: Login): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<any>(this.loginUrl, user, { headers });
  }

  public verifyCode(CodVerificacaoDTO: { login: string, code: string }): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<any>(this.verificarCodAutUrl, CodVerificacaoDTO, { headers });
  }

  public atualizarSenha(login: string, senha: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.patch<any>(this.atualizarSenhaUrl, { login, senha }, { headers });
  }

  public forgotPassword(email: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<any>(this.forgotPasswordUrl, JSON.stringify(email), { headers });
  }

  public isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }

  public getLoggedUserId(): string | null {
    return sessionStorage.getItem('login');
  }

  public GetUsersLoginByID(id: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.GetUsersLoginByIDUrl}?id=${id}`, { headers });
  }

  public GetUsersByLogin(login: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.GetUsersByLoginUrl}?login=${login}`, { headers });
  }

  logout() {
    localStorage.removeItem('currentUser');
    sessionStorage.removeItem('login');
    sessionStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
