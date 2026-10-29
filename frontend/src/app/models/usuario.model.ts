// Usuario tal como lo devuelve la API (sin contraseña)
export interface Usuario {
  id: number;
  username: string;
  email: string;
  estado: boolean;
  fechaCreacion: string;
}

export interface CreateUsuarioRequest {
  username: string;
  email: string;
  password: string;
}

// En la edición todos los campos son opcionales
export interface UpdateUsuarioRequest {
  username?: string;
  email?: string;
  password?: string;
  estado?: boolean;
}

export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}

export interface LoginResponse {
  message: string;
  token: string;
  user: Usuario;
}
