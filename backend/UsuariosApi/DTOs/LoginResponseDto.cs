namespace UsuariosApi.DTOs;

public class LoginResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserResponseDto User { get; set; } = new();
}
