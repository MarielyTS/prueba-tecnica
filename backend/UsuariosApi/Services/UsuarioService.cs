using UsuariosApi.DTOs;
using UsuariosApi.Exceptions;
using UsuariosApi.Models;
using UsuariosApi.Repositories;

namespace UsuariosApi.Services;

public class UsuarioService
{
    private readonly UsuarioRepository _repository;

    public UsuarioService(UsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.Select(ToResponse).ToList();
    }

    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var usuario = await FindOrThrow(id);
        return ToResponse(usuario);
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var username = dto.Username.Trim();
        var email = dto.Email.Trim().ToLower();

        if (await _repository.UsernameExistsAsync(username))
            throw new AppException("El username ya está registrado");

        if (await _repository.EmailExistsAsync(email))
            throw new AppException("El email ya está registrado");

        var usuario = new Usuario
        {
            Username = username,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Estado = true
        };

        // EF lee de vuelta id y fecha_creacion generados por la BD
        await _repository.AddAsync(usuario);
        return ToResponse(usuario);
    }

    public async Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        var usuario = await FindOrThrow(id);

        if (!string.IsNullOrWhiteSpace(dto.Username))
        {
            var username = dto.Username.Trim();
            if (await _repository.UsernameExistsAsync(username, id))
                throw new AppException("El username ya está registrado");
            usuario.Username = username;
        }

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var email = dto.Email.Trim().ToLower();
            if (await _repository.EmailExistsAsync(email, id))
                throw new AppException("El email ya está registrado");
            usuario.Email = email;
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        if (dto.Estado.HasValue)
            usuario.Estado = dto.Estado.Value;

        await _repository.UpdateAsync(usuario);
        return ToResponse(usuario);
    }

    // Borrado lógico: se marca el usuario como inactivo
    public async Task DeactivateAsync(int id)
    {
        var usuario = await FindOrThrow(id);
        usuario.Estado = false;
        await _repository.UpdateAsync(usuario);
    }

    private async Task<Usuario> FindOrThrow(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
            throw new AppException($"No se encontró el usuario con id {id}", 404);
        return usuario;
    }

    public static UserResponseDto ToResponse(Usuario u)
    {
        return new UserResponseDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Estado = u.Estado,
            FechaCreacion = u.FechaCreacion
        };
    }
}
