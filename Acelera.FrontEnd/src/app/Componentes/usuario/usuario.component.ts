import { AuthenticationService } from './../../Services/authentication.service';
import { Component, HostListener, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { MenuHamburguerComponent } from '../menu-hamburguer/menu-hamburguer.component';
import { CommonModule } from '@angular/common';
import { MenuService } from '../../Services/menu.service';
import { UserService } from '../../Services/user.service';
import { ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Usuario } from '../../Models/usuario';
import { Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-usuario',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, MenuHamburguerComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  menuOpen = false;
  allProfiles: any[] = [];
  perfisSelecionados: number[] = []; 
  allUnidades: any[] = [];
  usuarioForm!: FormGroup;
  erroPerfil: boolean = false;
  isConsultaVar: boolean = false;
  labelBotaoSubmit: string = 'CADASTRAR';
  labelBotaoVoltar = 'CANCELAR';
  labelBotaoInativar = 'INATIVAR';
  btEditar: boolean = false;
  btVoltar: boolean = false;
  isEdit: boolean = false;
  btInativar: boolean = false;
  loginInativo: boolean = false;

  imgIconCadUser = '../../../assets/users/cadastrar.svg';
  imgIconCadUserMob = '../../../assets/users/cadastrar_mob.svg';
  imgIconCadUserHover = '../../../assets/users/cadastrar_hover.svg';
  imgIconCadUserMobHover = '../../../assets/users/cadastrar_mob_hover.svg';

  imgIconConUser = '../../../assets/users/consultar.svg';
  imgIconConUserMob = '../../../assets/users/consultar_mob.svg';
  imgIconConUserHover = '../../../assets/users/consultar_hover.svg';
  imgIconConUserMobHover = '../../../assets/users/consultar_mob_hover.svg';

  currentImgCadUser: string = '';
  currentImgConUser: string = '';

  constructor(private menuService: MenuService, private userService: UserService, private fb: FormBuilder, private authenticationService: AuthenticationService) {}

  ngOnInit() {
    this.usuarioForm = this.fb.group({
      nomeCompleto: ['', [Validators.required, Validators.pattern('^[a-zA-Z\\s]*$')]], // Sem números
      login: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$')]], // Sem espaços ou caracteres especiais
      email: ['', [Validators.required, Validators.email]], // Validação de email
      unidade: ['', Validators.required] 
    });

    this.menuService.menuOpen$.subscribe(open => {
      this.menuOpen = open;
    });

    this.userService.getAllGrupoAcesso().subscribe({
      next: (profiles) => {
          this.allProfiles = profiles;
      },
      error: (err) => console.error('Erro ao buscar perfis:', err)
  });

    this.userService.getAllUnidades().subscribe({
      next: (unidades) => {
        this.allUnidades = unidades;
        if (this.allUnidades.length > 0) {
          this.usuarioForm.patchValue({ unidade: this.allUnidades[0].idunidadeinst });
        }
      },
      error: (err) => console.error('Erro ao buscar unidades:', err)
    });
  
    this.updateIcons();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateIcons();
  }
  
  onCheckboxChange(event: any, index: number) {
    if (this.isConsultaVar) return;

    const value = event.target.value;
    if (event.target.checked) {
        if (!this.perfisSelecionados.includes(value)) {
            this.perfisSelecionados.push(value);
        }
    } else {
        this.perfisSelecionados = this.perfisSelecionados.filter(profile => profile !== value && profile !== this.allProfiles[index]?.descgrupoacesso);
    }
}

  onSubmit() {
    if (this.isConsultaVar) {
      this.pesquisarUsuario();
    } else if (this.isEdit) {
      if (this.labelBotaoSubmit === 'SALVAR') {
        this.updateUsuario();
      } else {
        this.editarUsuario();
      }
    } else {
      this.cadastrarUsuario();
    }
  }

  cadastrarUsuario() {
    this.usuarioForm.get('nomeCompleto')?.setValidators([Validators.required, Validators.pattern('^[a-zA-Z\\s]*$')]);
    this.usuarioForm.get('login')?.setValidators([Validators.required, Validators.pattern('^[a-zA-Z0-9]+$')]);
    this.usuarioForm.get('email')?.setValidators([Validators.required, Validators.email]);

    this.usuarioForm.updateValueAndValidity();

    if (this.usuarioForm.invalid || this.perfisSelecionados.length === 0) {
      this.usuarioForm.markAllAsTouched();
      this.erroPerfil = this.perfisSelecionados.length === 0;
      return;
    }
  
    this.erroPerfil = false;
  
    const userLogin = this.authenticationService.getLoggedUserId();
    if (!userLogin) {
        alert('Usuário não está logado.');
        return;
    }

    this.authenticationService.GetUsersByLogin(userLogin).subscribe({
        next: (usuarioLogado) => {
            const userId = usuarioLogado.idusuariosistema;
            if (!userId) {
                alert('ID do usuário logado não encontrado.');
                return;
            }

            const userData: Usuario = {
                idusuario: this.usuarioForm.value.login,
                idunidadeinst: this.usuarioForm.value.unidade,
                descnomeusuario: this.usuarioForm.value.nomeCompleto,
                descemail: this.usuarioForm.value.email,
                perfis: this.perfisSelecionados
            };

            const payload = {
                user: userData,
                listaGrupoAcesso: this.perfisSelecionados.map((perfil: any) => parseInt(perfil, 10)), 
                loginCriador: userId
            };

            this.userService.cadastrarUsuario(payload).subscribe({
                next: (response) => {
                    alert('Usuário cadastrado com sucesso');
                    this.resetForm();
                },
                error: (err) => {
                    alert(err.message);
                }
            });
        },
        error: (err) => {
            alert('Erro ao obter ID do usuário logado: ' + err.message);
        }
    });
  }

  pesquisarUsuario() {
    const nome = this.usuarioForm.get('nomeCompleto')?.value;
    const login = this.usuarioForm.get('login')?.value;
    const email = this.usuarioForm.get('email')?.value;

    if ((nome && (login || email)) || (login && email) || (!nome && !login && !email)) {
      alert('Apenas um dos campos deve ser preenchido para pesquisa.');
      return;
    }

    let observable: Observable<any>;

    if (nome) {
      observable = this.userService.getUserByNome(nome);
    } else if (login) {
      observable = this.userService.getUserByID(login);
    } else if (email) {
      observable = this.userService.getUserByEmail(email);
    } else {
      alert('Preencha um campo para pesquisa.');
      return;
    }

    observable.subscribe({
      next: (response) => {
        const userData = response.value;
        if (userData) {
          this.usuarioForm.patchValue({
            nomeCompleto: userData.descnomeusuario,
            login: userData.idusuario,
            email: userData.descemail,
            unidade: userData.idunidadeinst
          });
          this.perfisSelecionados = userData.perfis || [];
          this.preencherPerfisSelecionados(userData.perfis);
          this.disableFormFields();
          this.btVoltar = true;
          this.labelBotaoSubmit = 'EDITAR';
          this.isEdit = true;
          this.isConsultaVar = false;
          this.labelBotaoVoltar = 'VOLTAR';
          this.btInativar = true; 

          this.userService.VerificaLoginInativo(userData.idusuario).subscribe({
            next: (inativo) => {
              this.loginInativo = inativo;
              if (this.loginInativo) {
                this.disableFormFields();
                this.btEditar = false;
                this.btInativar = false;
                this.disableEditButton();
              } else {
                this.btEditar = true;
                this.btInativar = true;
                this.labelBotaoInativar = 'INATIVAR';
                this.enableEditButton();
              }
            },
            error: (err) => {
              console.error('Erro ao verificar se o login está inativo:', err);
            }
          });
        } else {
          alert('Usuário não encontrado!');
        }
      },
      error: (err) => {
        if (err.status === 404) {
          alert('Usuário não encontrado!');
        } else {
          alert('Erro ao buscar usuário: ' + err.message);
        }
      }
    });
  }      

  preencherPerfisSelecionados(perfis: number[]) {
    if (!perfis) return;

    this.allProfiles.forEach(profile => {
      const checkbox = document.getElementById(profile.idgrupoacesso.toString()) as HTMLInputElement;
      if (checkbox) {
        checkbox.checked = perfis.includes(profile.idgrupoacesso);
      }
    });
  }

  convertToUpperCase(controlName: string) {
    const control = this.usuarioForm.get(controlName);
    if (control) {
      control.setValue(control.value.toUpperCase(), { emitEvent: false });
    }
  }

  updateUsuario() {
    if (this.usuarioForm.invalid || this.perfisSelecionados.length === 0) {
        this.usuarioForm.markAllAsTouched();
        this.erroPerfil = this.perfisSelecionados.length === 0;
        return;
    }
  
    this.usuarioForm.get('login')?.enable();
  
    const userLogin = this.authenticationService.getLoggedUserId();
    if (!userLogin) {
        alert('Usuário não está logado.');
        return;
    }
  
    this.authenticationService.GetUsersByLogin(userLogin).subscribe({
        next: (usuarioLogado) => {
            const userId = usuarioLogado.idusuariosistema;
            if (!userId) {
                alert('ID do usuário logado não encontrado.');
                return;
            }
  
            const userData: Usuario = {
                idusuario: this.usuarioForm.value.login.toUpperCase(), 
                idunidadeinst: this.usuarioForm.value.unidade.toUpperCase(),
                descnomeusuario: this.usuarioForm.value.nomeCompleto.toUpperCase(),
                descemail: this.usuarioForm.value.email.toUpperCase(),
                perfis: this.perfisSelecionados
            };
  
            // Separar perfis com descrições dos perfis com IDs
            const perfisComDescricoes = this.perfisSelecionados.filter(perfil => isNaN(Number(perfil)));
            const idsParaBuscar = this.perfisSelecionados.filter(perfil => !isNaN(Number(perfil)));
  
            if (idsParaBuscar.length > 0) {
                const perfilObservables = idsParaBuscar.map(id => this.userService.getPerfilByID(Number(id)));
                forkJoin(perfilObservables).subscribe({
                    next: (perfilResponses) => {
                        const perfisDescricoes = perfilResponses.map(perfil => perfil.descgrupoacesso);
                        const perfisFinal = [...perfisComDescricoes, ...perfisDescricoes];
  
                        const payload = {
                            user: {
                                ...userData,
                                perfis: perfisFinal
                            },
                            loginAlteradoPor: userId
                        };
  
                        this.userService.updateUsuario(payload).subscribe({
                            next: (response) => {
                                alert('Usuário atualizado com sucesso');
                                this.resetForm();
                                this.usuarioForm.get('nomeCompleto')?.enable();
                                this.usuarioForm.get('login')?.enable();
                                this.usuarioForm.get('email')?.enable();
                                this.btInativar = false;
                                this.isConsulta();
                                location.reload();
                            },
                            error: (err) => {
                                alert(err.message);
                            }
                        });
                    },
                    error: (err) => {
                        alert('Erro ao buscar descrições dos perfis: ' + err.message);
                    }
                });
            } else {
                const payload = {
                    user: {
                        ...userData,
                        perfis: perfisComDescricoes 
                    },
                    loginAlteradoPor: userId
                };
  
                this.userService.updateUsuario(payload).subscribe({
                    next: (response) => {
                        alert('Usuário atualizado com sucesso');
                        this.resetForm();
                        this.usuarioForm.get('nomeCompleto')?.enable();
                        this.usuarioForm.get('login')?.enable();
                        this.usuarioForm.get('email')?.enable();
                        this.btInativar = false;
                        this.isConsulta();
                        location.reload();
                    },
                    error: (err) => {
                        alert(err.message);
                    }
                });
            }
        },
        error: (err) => {
            alert('Erro ao obter ID do usuário logado: ' + err.message);
        }
    });
  }

  inativarUsuario() {
    if (confirm('Tem certeza que deseja inativar este usuário?')) {
        const userLogin = this.authenticationService.getLoggedUserId();
        if (!userLogin) {
            alert('Usuário não está logado.');
            return;
        }

        this.authenticationService.GetUsersByLogin(userLogin).subscribe({
            next: (usuarioLogado) => {
                const userId = usuarioLogado.idusuariosistema;
                if (!userId) {
                    alert('ID do usuário logado não encontrado.');
                    return;
                }

                const userData: Usuario = {
                    idusuario: this.usuarioForm.value.login,
                    idunidadeinst: this.usuarioForm.value.unidade,
                    descnomeusuario: this.usuarioForm.value.nomeCompleto,
                    descemail: this.usuarioForm.value.email,
                    perfis: this.perfisSelecionados
                };

                const payload = {
                    user: userData,
                    listaGrupoAcesso: this.perfisSelecionados.map((perfil: any) => parseInt(perfil, 10)),
                    loginInativadoPor: userId
                };

                this.userService.inativarUsuario(payload).subscribe({
                    next: (response) => {
                        alert('Usuário inativado com sucesso.');
                        this.btInativar = false;
                        this.disableFormFields();
                        location.reload();
                    },
                    error: (err) => {
                        alert(err.message);
                    }
                });
            },
            error: (err) => {
                alert('Erro ao obter ID do usuário logado: ' + err.message);
            }
        });
    }
}

  private disableEditButton() {
    const btnEditar = document.querySelector('.btProximo button') as HTMLButtonElement;
    if (btnEditar) {
      btnEditar.disabled = true;
      btnEditar.style.backgroundColor = 'darkgray';
    }
  }

  private enableEditButton() {
    const btnEditar = document.querySelector('.btProximo button') as HTMLButtonElement;
    if (btnEditar) {
      btnEditar.disabled = false;
      btnEditar.style.backgroundColor = '';
    }
  }
  

  voltar() {
    this.loginInativo = false;
    if (this.isEdit) {
      if (this.labelBotaoSubmit === 'SALVAR') {
        this.disableFormFields();
        this.labelBotaoSubmit = 'EDITAR';
        this.labelBotaoVoltar = 'VOLTAR';
      } else {
        this.resetForm();
        this.isConsultaVar = true;
        this.labelBotaoSubmit = 'PESQUISAR';
        this.labelBotaoVoltar = 'VOLTAR';
        this.usuarioForm.get('nomeCompleto')?.enable();
        this.usuarioForm.get('login')?.enable();
        this.usuarioForm.get('email')?.enable();
      }
    } else {
      this.isConsulta();
      this.resetForm();
      this.labelBotaoSubmit = 'PESQUISAR';
      this.labelBotaoVoltar = 'VOLTAR';
      this.usuarioForm.get('nomeCompleto')?.enable();
      this.usuarioForm.get('login')?.enable();
      this.usuarioForm.get('email')?.enable();
    }

    const btnProximo = document.querySelector('.btProximo button') as HTMLButtonElement;
    if (btnProximo) {
      btnProximo.disabled = false;
      btnProximo.style.backgroundColor = '';
    }
  }
  
  private disableFormFields() {
    this.usuarioForm.get('nomeCompleto')?.disable();
    this.usuarioForm.get('login')?.disable();
    this.usuarioForm.get('email')?.disable();
    this.usuarioForm.get('unidade')?.disable();
    this.allProfiles.forEach(profile => {
      const checkbox = document.getElementById(profile.idgrupoacesso.toString()) as HTMLInputElement;
      if (checkbox) {
        checkbox.disabled = true;
      }
    });
  }
  
  private enableFormFields() {
    this.usuarioForm.get('nomeCompleto')?.enable();
    this.usuarioForm.get('login')?.enable();
    this.usuarioForm.get('email')?.enable();
    this.usuarioForm.get('unidade')?.enable();
    this.allProfiles.forEach(profile => {
      const checkbox = document.getElementById(profile.idgrupoacesso.toString()) as HTMLInputElement;
      if (checkbox) {
        checkbox.disabled = false;
      }
    });
  }
  
  private resetForm() {
    this.usuarioForm.reset();
    this.enableFormFields();
    this.btVoltar = false;
    this.isEdit = false;
    this.clearPerfisSelecionados();
    this.labelBotaoVoltar = 'VOLTAR';
    this.labelBotaoSubmit = 'PESQUISAR';
    this.disableFormFields(); 
  }
  
  editarUsuario() {
    this.enableFormFields();
    this.usuarioForm.get('login')?.disable();
    this.labelBotaoSubmit = 'SALVAR';
    this.btVoltar = true;
    this.labelBotaoVoltar = 'CANCELAR';
  }

  clearPerfisSelecionados() {
    this.perfisSelecionados = [];
    this.allProfiles.forEach(profile => {
      const checkbox = document.getElementById(profile.idgrupoacesso.toString()) as HTMLInputElement;
      if (checkbox) {
        checkbox.checked = false;
        checkbox.disabled = true; 
      }
    });
  }

  get nomeCompleto() {
    return this.usuarioForm.get('nomeCompleto');
  }

  get login() {
    return this.usuarioForm.get('login');
  }

  get email() {
    return this.usuarioForm.get('email');
  }

  get unidade() {
    return this.usuarioForm.get('unidade');
  }

  isConsulta() {
    this.isConsultaVar = true;
    this.labelBotaoSubmit = 'PESQUISAR';
    this.updateIcons();
    this.enableFormFields();
    this.usuarioForm.get('unidade')?.disable();
    this.usuarioForm.get('perfis')?.disable();
    this.btVoltar = false;
    this.usuarioForm.reset();
    this.loginInativo = false;

    const btnProximo = document.querySelector('.btn-next') as HTMLButtonElement;
    if (btnProximo) {
        btnProximo.disabled = false;
        btnProximo.style.backgroundColor = '';
    }
  }

  isCadastro() {
    this.isConsultaVar = false;
    this.labelBotaoSubmit = 'CADASTRAR';
    this.updateIcons();
    this.usuarioForm.get('unidade')?.enable();
    this.usuarioForm.get('perfis')?.enable();
    this.usuarioForm.reset();
    this.usuarioForm.patchValue({ unidade: this.allUnidades[0]?.idunidadeinst });
    this.btVoltar = false;
    this.loginInativo = false;

    const btnProximo = document.querySelector('.btn-next') as HTMLButtonElement;
    if (btnProximo) {
        btnProximo.disabled = false;
        btnProximo.style.backgroundColor = '';
    }

    
  }

  updateIcons() {
    const isMobile = window.innerWidth <= 992;
    if (this.isConsultaVar) {
      this.currentImgCadUser = isMobile ? this.imgIconCadUserMob : this.imgIconCadUser;
      this.currentImgConUser = isMobile ? this.imgIconConUserMobHover : this.imgIconConUserHover;
    } else {
      this.currentImgCadUser = isMobile ? this.imgIconCadUserMobHover : this.imgIconCadUserHover;
      this.currentImgConUser = isMobile ? this.imgIconConUserMob : this.imgIconConUser;
    }

    const cadastroIcon = document.querySelector('.icon-cadastro') as HTMLImageElement;
    const consultaIcon = document.querySelector('.icon-consulta') as HTMLImageElement;
    if (cadastroIcon) cadastroIcon.src = this.currentImgCadUser;
    if (consultaIcon) consultaIcon.src = this.currentImgConUser;
  }
}