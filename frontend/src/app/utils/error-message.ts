import { HttpErrorResponse } from '@angular/common/http';

// Convierte un error HTTP de la API en un mensaje legible
export function getErrorMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'No se pudo conectar con el servidor. ¿Está la API en ejecución?';
  }

  const body = error.error;

  // Errores de validación (400) traen una lista en "errors"
  if (body?.errors?.length) {
    return body.errors.join(' ');
  }

  return body?.message ?? 'Ocurrió un error inesperado.';
}
