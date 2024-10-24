import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private getAllLogsUrl = `${environment.apiUrlParam}/Log/GetAllLogs`;

  
  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  public getAllLogs(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(this.getAllLogsUrl, { headers });
  }
}
