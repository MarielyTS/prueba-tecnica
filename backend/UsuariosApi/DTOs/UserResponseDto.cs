namespace UsuariosApi.DTOs;

// Lo que se devuelve al cliente (sin la contraseña)
public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
}
