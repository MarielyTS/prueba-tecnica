using Microsoft.AspNetCore.Mvc;
using UsuariosApi.DTOs;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsersController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // GET /api/users
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAll()
    {
        return Ok(await _usuarioService.GetAllAsync());
    }

    // GET /api/users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetById(int id)
    {
        return Ok(await _usuarioService.GetByIdAsync(id));
    }

    // POST /api/users
    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto dto)
    {
        var usuario = await _usuarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    // PUT /api/users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> Update(int id, UpdateUserDto dto)
    {
        return Ok(await _usuarioService.UpdateAsync(id, dto));
    }

    // DELETE /api/users/{id}  (desactiva el usuario, no lo borra)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _usuarioService.DeactivateAsync(id);
        return Ok(new { message = "Usuario desactivado correctamente" });
    }
}
