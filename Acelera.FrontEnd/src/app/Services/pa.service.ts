import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GraficoPA } from '../Models/graficoPA';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PaService {
  private NumerariosUrl = `${environment.apiUrlNum}/Numerarios/SaldoDeTerminais`;
  private NumerariosPeriodoUrl = `${environment.apiUrlNum}/Numerarios/SaldoPorPeriodo`;

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  public getSaldoDeTerminais(idPa: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.NumerariosUrl}${idPa}`, { headers });
  }

  getSaldosPorPeriodo(request: { idPa: string, numTerminal: number, inicio: Date, fim: Date }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post<any>(`${this.NumerariosPeriodoUrl}`, request, { headers });
  }
}
