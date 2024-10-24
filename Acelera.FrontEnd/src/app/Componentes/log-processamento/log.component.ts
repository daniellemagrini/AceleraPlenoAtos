import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../Services/menu.service';
import { AuthenticationService } from '../../Services/authentication.service';
import { MenuHamburguerComponent } from "../menu-hamburguer/menu-hamburguer.component";
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LogService } from '../../Services/log.service';
import { Log } from '../../Models/log';
import { FormsModule } from '@angular/forms';


@Component({
    selector: 'app-log',
    standalone: true,
    templateUrl: './log.component.html',
    styleUrl: './log.component.css',
    imports: [MenuHamburguerComponent, CommonModule, RouterOutlet, FormsModule]
})
export class LogComponent implements OnInit {
  menuOpen = false;
  logs: Log[] = [];
  filtroLogs: Log[] = [];
  showFilters = false;
  statusFilter: string = '';
  dateFilter: string = '';
  statusOptions: string[] = [];

  constructor(private menuService: MenuService, private authService: AuthenticationService, private logService: LogService) {}

  ngOnInit() {
    this.menuService.menuOpen$.subscribe(open => {
      this.menuOpen = open;
    });

    this.carregaLogs();
  }

  carregaLogs() {
    this.logService.getAllLogs().subscribe({
      next: (data) => {
        this.logs = data;
        this.filtroLogs = data;
        this.statusOptions = this.getStatus(data);
      },
      error: (err) => {
        console.error('Erro ao carregar logs', err);
      }
    });
  }

  corStatus(status: string): string {
    switch (status) {
      case 'OK':
        return '#1d7f2c';
      case 'AVISO':
        return '#f3a30e';
      case 'ERRO':
        return '#871f1f';
      default:
        return '#000000';
    }
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  aplicarFiltro() {
    this.filtroLogs = this.logs.filter(log => {
      const matchesStatus = this.statusFilter ? log.status === this.statusFilter : true;
      const logDate = log.datahoralog ? new Date(log.datahoralog) : undefined;
      const filterDate = this.dateFilter ? new Date(this.dateFilter) : undefined;
      const matchesDate = logDate && filterDate ? this.isSameDate(logDate, filterDate) : true;
      return matchesStatus && matchesDate;
    });
  }

  isSameDate(date1: Date, date2: Date): boolean {
    return date1.toISOString().split('T')[0] === date2.toISOString().split('T')[0];
  }

  resetarFiltro() {
    this.statusFilter = '';
    this.dateFilter = '';
    this.filtroLogs = this.logs;
    this.carregaLogs();
  }

  getStatus(logs: Log[]): string[] {
    const statusSet = new Set(logs.map(log => log.status));
    return Array.from(statusSet);
  }
}
