namespace UsuariosApi.Exceptions;

// Excepción con código HTTP para errores controlados (400, 401, 404...)
public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }
}
