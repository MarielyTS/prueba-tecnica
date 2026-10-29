import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Usuario } from '../../../models/usuario.model';
import { UserService } from '../../../services/user.service';
import { getErrorMessage } from '../../../utils/error-message';

@Component({
  selector: 'app-user-list',
  imports: [RouterLink, DatePipe],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  private userService = inject(UserService);

  users = signal<Usuario[]>([]);
  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.userService.getAll().subscribe({
      next: (users) => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(getErrorMessage(err));
        this.loading.set(false);
      }
    });
  }

  deleteUser(user: Usuario): void {
    if (!confirm(`¿Seguro que deseas eliminar al usuario "${user.username}"?`)) {
      return;
    }

    this.successMessage.set('');
    this.errorMessage.set('');

    this.userService.delete(user.id).subscribe({
      next: (response) => {
        this.successMessage.set(response.message);
        this.loadUsers();
      },
      error: (err: HttpErrorResponse) => this.errorMessage.set(getErrorMessage(err))
    });
  }
}
