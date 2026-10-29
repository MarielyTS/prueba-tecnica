using UsuariosApi.DTOs;
using UsuariosApi.Exceptions;
using UsuariosApi.Repositories;

namespace UsuariosApi.Services;

public class AuthService
{
    private readonly UsuarioRepository _repository;

    public AuthService(UsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await _repository.GetByUsernameOrEmailAsync(dto.UsernameOrEmail.Trim());

        // Mismo mensaje si no existe o si la contraseña es incorrecta
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
            throw new AppException("Usuario o contraseña incorrectos", 401);

        if (!usuario.Estado)
            throw new AppException("El usuario está inactivo", 401);

        return new LoginResponseDto
        {
            Message = "Login exitoso",
            // Token simple (no JWT): identificador aleatorio para la sesión del frontend
            Token = Guid.NewGuid().ToString(),
            User = UsuarioService.ToResponse(usuario)
        };
    }
}
