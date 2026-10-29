using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.DTOs;

public class LoginDto
{
    // Acepta username o email
    [Required(ErrorMessage = "El username o email es obligatorio")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}
