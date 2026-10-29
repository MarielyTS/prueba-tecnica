using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosApi.Models;

[Table("usuarios")]
public class Usuario
{
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("estado")]
    public bool Estado { get; set; } = true;

    // La genera la base de datos con DEFAULT NOW()
    // La columna es TIMESTAMP (sin zona horaria), hay que indicarlo para que Npgsql no use timestamptz
    [Column("fecha_creacion", TypeName = "timestamp without time zone")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime FechaCreacion { get; set; }
}
