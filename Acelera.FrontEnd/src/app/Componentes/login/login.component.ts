import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { Login } from '../../Models/login';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, RouterModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "hide";

  loginForm!: FormGroup;
  verifyCodeForm!: FormGroup;
  forgotPasswordForm!: FormGroup;

  loginDto = new Login();

  constructor(private authService: AuthenticationService, private fb: FormBuilder, private router: Router){}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.verifyCodeForm = this.fb.group({
      code: ['', Validators.required]
    });

    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  togglePasswordVisibility() {
    this.isText = !this.isText;
    this.type = this.isText ? "text" : "password";
  }


  onLogin() {
    if (this.loginForm.valid) {
      this.loginDto.login = this.loginForm.get('username')?.value;
      this.loginDto.senha = this.loginForm.get('password')?.value;
      this.authService.login(this.loginDto).subscribe({
        next: (response) => {
          if (response.tempLogin) {
            sessionStorage.setItem('tempLogin', this.loginDto.login);
            const modal = document.getElementById('modalAcessarCodVerificacao');
            if (modal) {
              modal.classList.add('show');
              modal.style.display = 'block';
            }
          } else {
            alert("Login e/ou Senha inválidos!");
          }
        },
        error: (err) => {
          alert("Erro ao fazer login. Verifique suas credenciais.");
        }
      });
    } else {
      console.log('Formulário inválido:', this.loginForm);
      this.loginForm.markAllAsTouched();
      alert("Login e/ou Senha inválidos!");
    }
  }

  onVerifyCode() {
    if (this.verifyCodeForm.valid) {
        const code = this.verifyCodeForm.get('code')?.value;
        const login = sessionStorage.getItem('tempLogin');
        console.log("Verifying code for login:", login);

        if (login) {
            this.authService.verifyCode({ login, code }).subscribe({
                next: (response) => {
                  console.log("Verify code response:", response);
                    if (response.token) {
                        sessionStorage.setItem('token', response.token);
                        sessionStorage.setItem('login', login);
                        sessionStorage.removeItem('tempLogin');
                        this.router.navigate(['/home']);
                    } else {
                        alert("Código inválido!");
                    }
                },
                error: (err) => {
                    console.error("Error verifying code:", err);
                    alert("Erro ao verificar código. Tente novamente.");
                }
            });
        } else {
            alert("Login não encontrado. Faça login novamente.");
        }
    } else {
        this.verifyCodeForm.markAllAsTouched();
        alert("Código inválido!");
    }
  }

  onCodeInput(event: any) {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, ''); // Remove todos os caracteres não numéricos
    input.value = value;
    this.verifyCodeForm.controls['code'].setValue(value);
  }

  onForgotPassword() {
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.get('email')?.value;
      this.authService.forgotPassword(email).subscribe({
        next: (response) => {
          alert(response.message);
          const modal = document.getElementById('modalEsqueceuSenha') as HTMLElement;
          const modalBackdrop = document.querySelector('.modal-backdrop') as HTMLElement;
          if (modal && modalBackdrop) {
            modal.classList.remove('show');
            modal.style.display = 'none';
            modalBackdrop.remove();
          }
          this.router.navigate(['/login']);
        },
        error: (err) => {
          alert(err.error.message || "Erro ao enviar e-mail. Verifique se o e-mail está correto.");
        }
      });
    } else {
      this.forgotPasswordForm.markAllAsTouched();
      alert("Por favor, insira um e-mail válido.");
    }
  }
}  
