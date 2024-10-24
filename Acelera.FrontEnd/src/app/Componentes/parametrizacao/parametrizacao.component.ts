import { Component, HostListener, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { MenuHamburguerComponent } from '../menu-hamburguer/menu-hamburguer.component';
import { CommonModule } from '@angular/common';
import { MenuService } from '../../Services/menu.service'
import { ParametrizacaoService } from '../../Services/parametrizacao.service';
import { FormsModule } from '@angular/forms';
import { Parametrizacao } from '../../Models/parametrizacao';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-parametrizacao',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, MenuHamburguerComponent, CommonModule, FormsModule],
  templateUrl: './parametrizacao.component.html',
  styleUrl: './parametrizacao.component.css'
})
export class ParametrizacaoComponent implements OnInit {
  menuOpen = false;
  parametrizacoes: Parametrizacao[] = [];
  indexExpandido: number | null = null;
  isEdit = false;
  isInativo = false;

  ImgEditUser = '../../../assets/param/editar.svg';
  ImgEditUserMob = '../../../assets/param/editarMob.svg';
  ImgEditUserHover = '../../../assets/param/editar_hover.svg';
  ImgEditUserMobHover = '../../../assets/param/editarMob_hover.svg';

  currentImgEditUser: string = '';

  constructor(private menuService: MenuService, private parametrizacaoService: ParametrizacaoService, private authService: AuthenticationService) {}

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateIcons();
  }

  ngOnInit() {
    this.menuService.menuOpen$.subscribe(open => {
      this.menuOpen = open;
    });

    this.loadParametrizacoes();
    this.updateIcons();
  }

  loadParametrizacoes() {
    this.parametrizacaoService.getAllParametrizacao().subscribe(
      (data) => {
        this.parametrizacoes = data;
      },
      (error) => {
        console.error('Erro ao carregar as parametrizações', error);
      }
    );
  }

  loadParametrizacaoIndividual(index: number): void {
    const param = this.parametrizacoes[index];
    this.parametrizacaoService.getParametrizacaoById(param.idparametrizacao).subscribe(
      (data) => {
        this.parametrizacoes[index] = { ...this.parametrizacoes[index], ...data };
        this.checkParametrizacaoInativa(index);
      },
      (error: HttpErrorResponse) => {
        alert('Erro ao carregar a parametrização.');
      }
    );
  }

  toggleExpand(index: number): void {
    if (this.indexExpandido === index) {
      this.indexExpandido = null; // Volta o que estava expandido ao normal
      this.disableEdit();
    } else {
      this.indexExpandido = index; // Expandir o novo índice
      this.disableEdit();     
      this.loadParametrizacaoIndividual(index); 
    }
  }

  checkParametrizacaoInativa(index: number): void {
    const param = this.parametrizacoes[index];
    if (!param || !param.idparametrizacao) {
      return;
    }
    this.parametrizacaoService.verificaParametrizacaoInativa(param.idparametrizacao).subscribe(
      (isInativo: boolean) => {
        if (this.indexExpandido === index) {
          this.isInativo = isInativo;
          if (this.isInativo) {
            this.disableEdit();
          }
        }
      },
      (error: HttpErrorResponse) => {
        alert('Erro ao verificar se a parametrização está inativa.');
      }
    );
  }
  
  cliqueProcessos(event: Event, index: number): void {
    event.stopPropagation();
    this.toggleExpand(index);
  }

  enableEdit(event: Event): void {
    event.stopPropagation();
    if (this.indexExpandido !== null && !this.isInativo) {
      this.isEdit = true;
      this.updateIcons();
    }
  }

  disableEdit(): void {
    this.isEdit = false;
    this.updateIcons();
  }

  updateIcons() {
    const isMobile = window.innerWidth <= 992;
    if (this.isEdit) {
      this.currentImgEditUser = isMobile ? this.ImgEditUserMobHover : this.ImgEditUserHover;
    } else {
      this.currentImgEditUser = isMobile ? this.ImgEditUserMob : this.ImgEditUser;
    }

    const editIcon = document.querySelector('.icon-edit') as HTMLImageElement;
    if (editIcon) editIcon.src = this.currentImgEditUser;
  }

  onMouseOver() {
    const isMobile = window.innerWidth <= 992;
    if (this.isEdit) {
      this.currentImgEditUser = isMobile ? this.ImgEditUserMobHover : this.ImgEditUserHover;
    } else {
      this.currentImgEditUser = isMobile ? this.ImgEditUserMobHover : this.ImgEditUserHover;
    }
  }

  onMouseOut() {
    this.updateIcons();
  }

  salvarAlteracoes(): void {
    if (this.indexExpandido !== null) {
      const param = this.parametrizacoes[this.indexExpandido];
      const userLogin = this.authService.getLoggedUserId();
      if (!userLogin) {
        alert('Usuário não está logado.');
        return;
      }
  
      this.authService.GetUsersByLogin(userLogin).subscribe({
        next: (usuarioLogado) => {
          const userId = usuarioLogado.idusuariosistema;
          if (!userId) {
            alert('ID do usuário logado não encontrado.');
            return;
          }
  
          this.parametrizacaoService.updateParametrizacao(param, userId).subscribe(
            () => {
              alert('Parametrização atualizada com sucesso!');
              this.disableEdit();
            },
            (error: HttpErrorResponse) => {
              console.error('Erro ao atualizar a parametrização', error);
              alert('Erro ao atualizar a parametrização.');
            }
          );
        },
        error: (err) => {
          alert('Erro ao obter ID do usuário logado: ' + err.message);
        }
      });
    }
  }

  voltar(): void {
    this.loadParametrizacoes(); 
    this.disableEdit();
  }

  inativarParametrizacao(): void {
    if (confirm('Tem certeza que deseja inativar esta parametrização?')) {
      if (this.indexExpandido !== null) {
        const param = this.parametrizacoes[this.indexExpandido];
        const userLogin = this.authService.getLoggedUserId();
        if (!userLogin) {
          alert('Usuário não está logado.');
          return;
        }
  
        this.authService.GetUsersByLogin(userLogin).subscribe({
          next: (usuarioLogado) => {
            const userId = usuarioLogado.idusuariosistema;
            if (!userId) {
              alert('ID do usuário logado não encontrado.');
              return;
            }
  
            this.parametrizacaoService.inativarParametrizacao(param.idparametrizacao, userId).subscribe(
              () => {
                alert('Parametrização inativada com sucesso!');
                location.reload();               
              },
              (error: HttpErrorResponse) => {
                console.error('Erro ao inativar a parametrização', error);
                alert('Erro ao inativar a parametrização.');
              }
            );
          },
          error: (err) => {
            alert('Erro ao obter ID do usuário logado: ' + err.message);
          }
        });
      }
    }
  }
}