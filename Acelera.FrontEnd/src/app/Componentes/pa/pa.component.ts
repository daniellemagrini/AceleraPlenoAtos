import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../Services/menu.service';
import { AuthenticationService } from '../../Services/authentication.service';
import { MenuHamburguerComponent } from "../menu-hamburguer/menu-hamburguer.component";
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../Services/user.service';
import { PaService } from '../../Services/pa.service';
import * as XLSX from 'xlsx';
import { Chart, registerables } from 'chart.js';
import 'chartjs-adapter-date-fns';
import { GraficoPA } from '../../Models/graficoPA';
import Swal from 'sweetalert2';
import annotationPlugin from 'chartjs-plugin-annotation';

Chart.register(...registerables);
Chart.register(annotationPlugin);

@Component({
  selector: 'app-pa',
  standalone: true,
  imports: [MenuHamburguerComponent, CommonModule, RouterOutlet, FormsModule, ReactiveFormsModule],
  templateUrl: './pa.component.html',
  styleUrl: './pa.component.css'
})
export class PaComponent implements OnInit {
  menuOpen = false;
  allUnidades: any[] = [];
  paForm!: FormGroup;
  dataAtualizacao: Date | undefined;
  saldoTotal: number = 0;
  terminais: any[] = [];
  tiposTerminais: string[] = [];
  filtroDataInicio: string = '';
  filtroDataFim: string = '';
  terminaisComGraficoVisivel: Set<number> = new Set();
  charts: { [key: number]: Chart } = {};

  imgExportar = '../../../assets/pa/exportar.svg';
  imgExportarHover = '../../../assets/pa/exportar_hover.svg';
  imgGrafico = '../../../assets/pa/grafico.svg';

  currentImgExportar: string = this.imgExportar;
  currentImgGrafico: string = this.imgGrafico;

  constructor(private menuService: MenuService, private authService: AuthenticationService,
      private userService: UserService, private fb: FormBuilder, private paService: PaService) {}

      ngOnInit() {
        this.paForm = this.fb.group({
          unidade: ['', Validators.required] 
        });
    
        this.menuService.menuOpen$.subscribe(open => {
          this.menuOpen = open;
        });
    
        this.userService.getAllUnidades().subscribe({
          next: (unidades) => {
            this.allUnidades = unidades;
            this.setUnidadeUsuarioLogado();
            if (this.allUnidades.length > 0) {
              this.dataAtualizacao = this.parseDate(this.allUnidades[0].datahoracarga);
            }
          },
          error: (err) => console.error('Erro ao buscar unidades:', err)
        });
        this.paForm.get('unidade')?.valueChanges.subscribe(selectedUnidadeId => {
          this.updateSaldoTotal(selectedUnidadeId);
        });
      }
    
      setUnidadeUsuarioLogado() {
        const userLogin = this.authService.getLoggedUserId();
        if (userLogin) {
          this.userService.getUnidadeByLogin(userLogin).subscribe({
            next: (data) => {
              const unidadeUsuario = data.unidade;
              const selectedUnidade = this.allUnidades.find(u => u.nomeunidade === unidadeUsuario);
              if (selectedUnidade) {
                this.paForm.patchValue({ unidade: selectedUnidade.idunidadeinst });
                this.updateSaldoTotal(selectedUnidade.idunidadeinst);
              }
            },
            error: (err) => console.error('Erro ao buscar unidade do usuário logado:', err)
          });
        }
      }

      updateSaldoTotal(unidadeId: string) {
        this.paService.getSaldoDeTerminais(unidadeId).subscribe({
          next: (response) => {
            this.saldoTotal = response.saldoTotal;
            this.dataAtualizacao = response.ultimaAtualizacao ? new Date(response.ultimaAtualizacao) : new Date(0);
            this.terminais = response.terminal;
            this.setTiposTerminais();
          },
          error: (err) => {
            console.error('Erro ao buscar saldo de terminais:', err);
            this.saldoTotal = 0;
            this.dataAtualizacao = new Date(0);
            this.terminais = [];
            this.tiposTerminais = [];
          }
        });
      }

      setTiposTerminais() {
        const tipos = new Set(this.terminais.map(terminal => terminal.tipoTerminal));
        this.tiposTerminais = Array.from(tipos);
      }

      getTerminaisByTipo(tipo: string) {
        return this.terminais.filter(terminal => terminal.tipoTerminal === tipo);
      }

      alterarImgExportar(hover: boolean) {
        this.currentImgExportar = hover ? this.imgExportarHover : this.imgExportar;
      }

      parseDate(dateString: string | undefined): Date | undefined {
        if (!dateString) {
          return undefined;
        }
        const date = new Date(dateString);
        return isNaN(date.getTime()) ? undefined : date;
      }
      
      getSaldoCor(saldo: number, limiteMin: number, limiteMax: number): string {
        if (saldo < limiteMin) {
          return '#871f1f';
        } else if (saldo > limiteMax) {
          return '#f3a30e';
        } else {
          return '#1d7f2c';
        }
      }

      exportarExcel() {
        const terminaisComGraficos = this.terminais.filter(terminal => this.isGraficoVisible(terminal.numTerminal));
        
        if (terminaisComGraficos.length > 0) {
          Swal.fire({
            title: 'Existem gráficos abertos',
            text: 'Deseja exportar com os dados dos gráficos também?',
            icon: 'warning',
            showCancelButton: true,
            showDenyButton: true,
            confirmButtonText: 'Sim',
            denyButtonText: 'Não',
            cancelButtonText: 'Cancelar'
          }).then((result) => {
            if (result.isConfirmed) {
              this.exportarComGraficos(true);
            } else if (result.isDenied) {
              this.exportarComGraficos(false);
            }
          });
        } else {
          this.exportarComGraficos(false);
        }
      }
      
      exportarComGraficos(incluirGraficos: boolean) {
        const data = [
          {
            'Unidade': this.paForm.get('unidade')?.value,
            'Saldo Total': this.saldoTotal,
            'Última Atualização': this.dataAtualizacao ? this.dataAtualizacao.toLocaleString() : ''
          },
          ...this.terminais.map(terminal => ({
            'Tipo de Terminal': terminal.tipoTerminal,
            'Terminal': terminal.numTerminal,
            'Usuário': terminal.usuario,
            'Saldo': terminal.saldo,
            'Limite Superior': terminal.limiteMax,
            'Limite Inferior': terminal.limiteMin
          }))
        ];
      
        const worksheet = XLSX.utils.json_to_sheet(data);
      
        if (incluirGraficos) {
          this.terminais.forEach(terminal => {
            if (this.isGraficoVisible(terminal.numTerminal)) {
              const chart = this.charts[terminal.numTerminal];
              if (chart) {
                const dadosGrafico = chart.data.datasets[0].data as number[];
                const labelsGrafico = chart.data.labels as string[];
      
                const graficoData = labelsGrafico.map((label, index) => ({
                  'Data': label,
                  'Saldo': dadosGrafico[index],
                  'Terminal': terminal.numTerminal,
                  'Usuário': terminal.usuario,
                  'Tipo de Terminal': terminal.tipoTerminal
                }));
      
                XLSX.utils.sheet_add_json(worksheet, graficoData, { skipHeader: true, origin: -1 });
              }
            }
          });
        }
      
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Pontos de Atendimento');
      
        XLSX.writeFile(workbook, 'Pontos_de_Atendimento.xlsx');
      }
           

      exportarGrafico(terminalId: number) {
        const chart = this.charts[terminalId];
        
        if (!chart) {
          alert('Sem dados para exportação');
          return;
        }
      
        const dadosGrafico = chart.data.datasets[0].data as number[];
        const labelsGrafico = chart.data.labels as string[];
      
        if (dadosGrafico.length === 0) {
          alert('Sem dados para exportação');
          return;
        }
      
        const terminal = this.terminais.find(t => t.numTerminal === terminalId);
      
        if (!terminal) {
          alert('Dados do terminal não encontrados');
          return;
        }
      
        const data = [
          {
            'Terminal': terminal.numTerminal,
            'Usuário': terminal.usuario,
            'Tipo de Terminal': terminal.tipoTerminal,
            'Limite Superior': terminal.limiteMax,
            'Limite Inferior': terminal.limiteMin
          },
          ...labelsGrafico.map((label, index) => ({
            'Data': label,
            'Saldo': dadosGrafico[index]
          }))
        ];
      
        const worksheet = XLSX.utils.json_to_sheet(data);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Gráfico de Saldos');
      
        XLSX.writeFile(workbook, `Grafico_Saldos_Terminal_${terminalId}.xlsx`);
      }

      toggleGrafico(terminalId: number) {
        if (this.terminaisComGraficoVisivel.has(terminalId)) {
          this.terminaisComGraficoVisivel.delete(terminalId);
        } else {
          this.terminaisComGraficoVisivel.add(terminalId);
        }
      }
    
      isGraficoVisible(terminalId: number): boolean {
        return this.terminaisComGraficoVisivel.has(terminalId);
      }
    
      carregarGrafico(terminalId: number) {
        const dataInicio = this.filtroDataInicio || this.getDataInicioPadrao();
        const dataFim = this.filtroDataFim || this.getDataFimPadrao();
    
        const request = {
          idPa: this.paForm.get('unidade')?.value,
          numTerminal: terminalId,
          inicio: new Date(dataInicio),
          fim: new Date(dataFim)
        };
    
        this.paService.getSaldosPorPeriodo(request).subscribe({
          next: (response) => {
            const dados: GraficoPA[] = response.datasOperacao;
            const terminal = this.terminais.find(t => t.numTerminal === terminalId);
            if (terminal) {
              this.renderizarGrafico(terminalId, dados, terminal.limiteMax, terminal.limiteMin);
            }
          },
          error: (err) => {
            if (err.status === 404) {
              alert('Não foram encontrados dados para o período selecionado.');
            } else if (err.status === 400) {
              alert('Solicitação inválida. Insira um período entre 3 e 30 dias');
            } else {
              console.error('Erro ao carregar dados do gráfico:', err);
            }
            this.removerGrafico(terminalId);
          }
        });
      }
    
      removerGrafico(terminalId: number) {
        if (this.charts[terminalId]) {
          this.charts[terminalId].destroy();
          delete this.charts[terminalId];
        }
      }
    
      getDataInicioPadrao(): string {
        const data = new Date();
        data.setDate(data.getDate() - 30);
        return data.toISOString().split('T')[0];
      }
    
      getDataFimPadrao(): string {
        return new Date().toISOString().split('T')[0];
      }
    
      renderizarGrafico(terminalId: number, dados: GraficoPA[], limiteMax: number, limiteMin: number) {
        const canvasId = `grafico-${terminalId}`;
        const canvas = document.getElementById(canvasId) as HTMLCanvasElement;
    
        if (canvas) {
          const ctx = canvas.getContext('2d');
          if (ctx) {
            if (this.charts[terminalId]) {
              this.charts[terminalId].destroy();
            }
    
            const saldos = dados.map(d => parseFloat(d.saldo));
            const minSaldo = Math.min(0, ...saldos);
            const maxSaldo = Math.max(...saldos);
    
            const labels = dados.map(d => new Date(d.data!).toLocaleDateString('pt-BR'));
            
            const yMax = Math.max(maxSaldo, limiteMax);
    
            this.charts[terminalId] = new Chart(ctx, {
              type: 'line',
              data: {
                labels: labels,
                datasets: [{
                  label: 'Saldo',
                  data: saldos,
                  borderColor: 'rgba(75, 192, 192, 1)',
                  borderWidth: 1
                }]
              },
              options: {
                scales: {
                  x: {
                    type: 'category',
                    labels: labels
                  },
                  y: {
                    min: minSaldo,
                    max: yMax,
                    beginAtZero: true,
                  }
                },
                plugins: {
                  tooltip: {
                    callbacks: {
                      title: (context) => context[0].label,
                    },
                  },
                  annotation: {
                    annotations: [
                      {
                        type: 'line',
                        yMin: limiteMax,
                        yMax: limiteMax,
                        borderColor: 'darkgray',
                        borderWidth: 2,
                        label: {
                          content: 'Limite Superior',
                          position: 'end'
                        }
                      },
                      {
                        type: 'line',
                        yMin: limiteMin,
                        yMax: limiteMin,
                        borderColor: 'darkgray',
                        borderWidth: 2,
                        label: {
                          content: 'Limite Inferior',
                          position: 'end'
                        }
                      }
                    ]
                  }
                }
              }
            });
          } else {
            console.error('Failed to get 2D context');
          }
        } else {
          console.error('Canvas element not found');
        }
      }
    }