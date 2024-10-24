import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-redefinicaoSenha',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, RouterModule, ReactiveFormsModule],
  templateUrl: './redefinicaoSenha.component.html',
  styleUrls: ['./redefinicaoSenha.component.css']
})
export class RedefinicaoSenhaComponent implements OnInit {
  type: string = "password";
  isText: boolean = false;

  loginForm!: FormGroup;

  constructor(private authService: AuthenticationService, private fb: FormBuilder, private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    const canAccess = sessionStorage.getItem('canAccessRedefinicaoSenha');
    if (canAccess !== 'true') {
      this.router.navigate(['/login']);
      return;
    }

    const login = this.route.snapshot.queryParams['login'] || '';

    this.loginForm = this.fb.group({
      username: [{ value: login, disabled: true }, Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), this.passwordStrengthValidator]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('password')!.value === form.get('confirmPassword')!.value ? null : { mismatch: true };
  }

  passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value.length < 6 || !/[A-Z]/.test(value) || !/[a-z]/.test(value) || !/[0-9]/.test(value) || !/[!@#$%&*.]/.test(value)) {
      return { weakPassword: true };
    }
    return null;
  }

  togglePasswordVisibility() {
    this.isText = !this.isText;
    this.type = this.isText ? "text" : "password";
  }

  onLogin() {
    if (this.loginForm.valid) {
      const login = this.loginForm.get('username')!.value;
      const senha = this.loginForm.get('password')!.value;

      this.authService.atualizarSenha(login, senha).subscribe(
        (response) => {
          alert(response.message);
          this.router.navigate(['/login']);
        },
        (error) => {
          if (error.status === 400) {
            alert('Erro ao criar a senha: Usuário não existente ou senha inválida.');
          } else {
            alert(`Erro ao criar a senha. Tente novamente mais tarde. Detalhes: ${error.message}`);
          }
        }
      );
    } else {
      this.loginForm.markAllAsTouched();
      this.showValidationErrors();
    }
  }

  showValidationErrors() {
    if (this.loginForm.hasError('weakPassword', 'password')) {
      alert('Senha tem que ter, no mínimo 6 digitos e ter: letras maiúsculas; letras minúsculas, números e caracter especial (!@#$%&*.)');
    } else if (this.loginForm.hasError('mismatch')) {
      alert('Senhas não coincidem!');
    } else {
      alert('Erro ao criar a senha.');
    }
  }

  navigateToRedefinicaoSenha() {
    sessionStorage.setItem('canAccessRedefinicaoSenha', 'true');
    this.router.navigate(['/redefinicaoSenha']);
  }
}