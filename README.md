# Prueba técnica Full Stack – Gestión de usuarios

Aplicación de login y CRUD de usuarios.

| Capa | Tecnología |
|---|---|
| Base de datos | PostgreSQL |
| Backend | .NET 8 (ASP.NET Core Web API + EF Core + BCrypt) |
| Frontend | Angular 22 (formularios reactivos, guard, CSS nativo) |

```
prueba-tecnica/
├── docker-compose.yml    -> PostgreSQL en Docker
├── database/script.sql   -> tabla usuarios + usuario de prueba
├── backend/UsuariosApi/  -> API REST (Controller → Service → Repository)
└── frontend/             -> aplicación Angular
```

## Requisitos

- Docker Desktop (para la base de datos)
- .NET 8 SDK
- Node.js 20 o superior

---

## 1. Base de datos (Docker Compose)

Desde la raíz del proyecto:

```bash
docker compose up -d
```

Esto levanta PostgreSQL 16 en `localhost:5432`.

- Base de datos: `prueba_tecnica`
- Usuario: `postgres`
- Contraseña: `postgres`

Al crearse el contenedor, ejecuta automáticamente `database/script.sql`, que crea la tabla `usuarios` e inserta el usuario de prueba.

Comprobar que los datos están cargados:

```bash
docker exec prueba-tecnica-db psql -U postgres -d prueba_tecnica -c "SELECT id, username, email, estado FROM usuarios;"
```

Otros comandos útiles:

| Acción | Comando |
|---|---|
| Detener (conserva los datos) | `docker compose down` |
| Reiniciar desde cero (vuelve a ejecutar el script) | `docker compose down -v` y luego `docker compose up -d` |

> El script solo se ejecuta cuando el volumen de datos está vacío. Si modificas `script.sql`, usa `docker compose down -v` para que se vuelva a aplicar.

La conexión ya está configurada en `backend/UsuariosApi/appsettings.json`, así que no hay que cambiar nada.

## 2. Backend (.NET)

```bash
cd backend/UsuariosApi
dotnet run
```

- API: http://localhost:5000/api
- Swagger: http://localhost:5000/swagger

## 3. Frontend (Angular)

```bash
cd frontend
npm install
ng serve        # o: npm start
```

Abrir http://localhost:4200.
La URL de la API está en `frontend/src/app/api.config.ts`.

---

## 4. Probar la aplicación

### Credenciales de prueba

| Usuario | Email | Contraseña |
|---|---|---|
| `admin` | `admin@test.com` | `Admin123` |

Se puede iniciar sesión con el usuario **o** con el email.

### Prueba rápida

1. Abre http://localhost:4200/users sin sesión: el **guard** te lleva a `/login`.
2. Inicia sesión con `admin` / `Admin123`: verás la tabla de usuarios.
3. **Crear:** pulsa *+ Nuevo usuario* y completa el formulario.
   Si repites el username o el email, la API muestra un error 400.
4. **Editar:** pulsa *Editar*, cambia el email y guarda.
   Si dejas vacía la contraseña, no se cambia.
5. **Eliminar:** pulsa *Eliminar* y confirma. El usuario queda **Inactivo** (borrado lógico) y ya no puede iniciar sesión.
   Puedes reactivarlo desde *Editar*.
6. **Cerrar sesión:** botón de la cabecera.

---

## API REST

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| POST | `/api/auth/login` | Login con `usernameOrEmail` y `password` | 200, 400, 401 |
| GET | `/api/users` | Listar usuarios | 200 |
| GET | `/api/users/{id}` | Obtener usuario | 200, 404 |
| POST | `/api/users` | Crear usuario (contraseña con BCrypt) | 201, 400 |
| PUT | `/api/users/{id}` | Actualizar usuario (campos opcionales) | 200, 400, 404 |
| DELETE | `/api/users/{id}` | Desactivar usuario | 200, 404 |

Ejemplos de cuerpo:

```json
POST /api/auth/login
{ "usernameOrEmail": "admin", "password": "Admin123" }

POST /api/users
{ "username": "maria", "email": "maria@test.com", "password": "Maria123" }

PUT /api/users/2
{ "email": "maria.nueva@test.com", "estado": true }
```

Todos los errores siguen el formato `{ "status": 404, "message": "..." }`.
Los errores de validación añaden además la lista `errors`.

## Decisiones técnicas

- **Contraseñas:** se guardan con hash BCrypt y la API nunca las devuelve.
- **Sesión:** el login devuelve un token simple (GUID), que Angular guarda en `localStorage`.
  - El `authGuard` protege las rutas `/users`.
  - No se usa JWT, para mantener la autenticación simple.
  - La API no valida el token: el guard solo protege la navegación en el frontend.
- **DELETE:** es un borrado lógico (`estado = false`), así se conserva el histórico.
- **CORS:** solo se permite el origen `http://localhost:4200`.
- **Emails:** se guardan en minúsculas, así que el login por email no distingue mayúsculas.
