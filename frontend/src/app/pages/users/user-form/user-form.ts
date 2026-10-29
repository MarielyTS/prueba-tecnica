import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { UpdateUsuarioRequest } from '../../../models/usuario.model';
import { UserService } from '../../../services/user.service';
import { getErrorMessage } from '../../../utils/error-message';

// Un mismo formulario sirve para crear (/users/new) y editar (/users/:id/edit)
@Component({
  selector: 'app-user-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './user-form.html'
})
export class UserForm implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  userId: number | null = null;
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    estado: [true]
  });

  get isEdit(): boolean {
    return this.userId !== null;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userId = Number(id);
      this.loadUser(this.userId);
    }
  }

  private loadUser(id: number): void {
    // Al editar, la contraseña es opcional (vacía = no se cambia)
    const password = this.form.controls.password;
    password.setValidators(Validators.minLength(6));
    password.updateValueAndValidity();

    this.userService.getById(id).subscribe({
      next: (user) => {
        this.form.patchValue({
          username: user.username,
          email: user.email,
          estado: user.estado
        });
      },
      error: (err: HttpErrorResponse) => this.errorMessage.set(getErrorMessage(err))
    });
  }

  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!control && control.touched && control.hasError(error);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.errorMessage.set('');

    const { username, email, password, estado } = this.form.getRawValue();

    const request$ = this.isEdit
      ? this.userService.update(this.userId!, this.buildUpdateRequest(username, email, password, estado))
      : this.userService.create({ username, email, password });

    request$.subscribe({
      next: () => {
        this.saving.set(false);
        this.router.navigate(['/users']);
      },
      error: (err: HttpErrorResponse) => {
        this.saving.set(false);
        this.errorMessage.set(getErrorMessage(err));
      }
    });
  }

  // Solo se envía la contraseña si el usuario escribió una nueva
  private buildUpdateRequest(username: string, email: string, password: string, estado: boolean): UpdateUsuarioRequest {
    const data: UpdateUsuarioRequest = { username, email, estado };
    if (password) {
      data.password = password;
    }
    return data;
  }
}
