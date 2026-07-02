namespace LaTiendaAPI.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RegisterRequestDto
    {
        public string TipoDoc { get; set; } = string.Empty;

        public string NroDoc { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public List<int> Roles { get; set; } = new List<int>();
    }
}
