using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.DTOs;

// Todos los campos son opcionales: solo se actualiza lo que se envía
public class UpdateUserDto
{
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El username debe tener entre 3 y 50 caracteres")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(100, ErrorMessage = "El email no puede superar 100 caracteres")]
    public string? Email { get; set; }

    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string? Password { get; set; }

    public bool? Estado { get; set; }
}
