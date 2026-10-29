import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { Login } from './pages/login/login';
import { UserForm } from './pages/users/user-form/user-form';
import { UserList } from './pages/users/user-list/user-list';

export const routes: Routes = [
  { path: 'login', component: Login },

  // Vistas de mantenimiento protegidas por el guard
  { path: 'users', component: UserList, canActivate: [authGuard] },
  { path: 'users/new', component: UserForm, canActivate: [authGuard] },
  { path: 'users/:id/edit', component: UserForm, canActivate: [authGuard] },

  { path: '', redirectTo: 'users', pathMatch: 'full' },
  { path: '**', redirectTo: 'users' }
];
