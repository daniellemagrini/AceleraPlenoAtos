import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Parametrizacao } from '../Models/parametrizacao';

@Injectable({
  providedIn: 'root'
})
export class ParametrizacaoService {
  private getAllParametrizacaoUrl = `${environment.apiUrlParam}/Param/GetAllParametrizacao`;
  private updateParametrizacaoUrl = `${environment.apiUrlParam}/Param/UpdateParametrizacao`;
  private inativarParametrizacaoUrl = `${environment.apiUrlParam}/Param/InativarParametrizacao`;
  private verificaParametrizacaoInativaUrl = `${environment.apiUrlParam}/Param/VerificaParametrizacaoInativa`;
  private getParametrizacaoByIdUrl = `${environment.apiUrlParam}/Param/GetParametrizacaoByID`;

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  public getAllParametrizacao(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(this.getAllParametrizacaoUrl, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  public getParametrizacaoById(id: number): Observable<Parametrizacao> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.get<Parametrizacao>(`${this.getParametrizacaoByIdUrl}`, { headers, params }).pipe(
      catchError(this.handleError)
    );
  }

  public updateParametrizacao(param: Parametrizacao, loginAlteradoPor: number): Observable<any> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('loginAlteradoPor', loginAlteradoPor.toString());
    return this.http.put<any>(this.updateParametrizacaoUrl, param, { headers, params }).pipe(
      catchError(this.handleError)
    );
  }

  public inativarParametrizacao(id: number, loginInativoPor: number): Observable<any> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams()
      .set('id', id.toString())
      .set('loginInativoPor', loginInativoPor.toString());
    return this.http.put<any>(this.inativarParametrizacaoUrl, {}, { headers, params }).pipe(
      catchError(this.handleError)
    );
  }

  public verificaParametrizacaoInativa(idParam: number): Observable<boolean> {
    if (idParam == null) {
      return throwError(() => new Error('idParam is null or undefined'));
    }
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('idParam', idParam.toString());
    return this.http.get<boolean>(this.verificaParametrizacaoInativaUrl, { headers, params }).pipe(
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
        errorMessage = 'Parametrização não encontrada!';
      } else if (error.status === 400 && error.error.message) {
        errorMessage = error.error.message;
      } else if (error.status === 500 && error.error.message) {
        errorMessage = error.error.message;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}