import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { getErrorMessage } from '../../utils/error-message';

// Si el usuario escribe un email (contiene "@"), se valida que sea un email válido
function usernameOrEmailValidator(control: AbstractControl): ValidationErrors | null {
  const value: string = control.value ?? '';
  if (value.includes('@')) {
    return Validators.email(control);
  }
  return null;
}

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    usernameOrEmail: ['', [Validators.required, usernameOrEmailValidator]],
    password: ['', Validators.required]
  });

  constructor() {
    // Si ya hay sesión, no tiene sentido mostrar el login
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/users']);
    }
  }

  // Muestra el error de un campo solo si el usuario ya lo tocó
  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!control && control.touched && control.hasError(error);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    this.authService.login(this.form.getRawValue()).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/users']);
      },
      error: (err: HttpErrorResponse) => {
        this.loading.set(false);
        this.errorMessage.set(getErrorMessage(err));
      }
    });
  }
}
