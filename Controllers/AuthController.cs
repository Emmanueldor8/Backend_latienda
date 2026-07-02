using LaTiendaApi.DTOs; // LoginRequestDto, LoginResponseDto, RegisterRequestDto
using LaTiendaApi.DTOs.Auth;
using LaTiendaApi.Helpers; // PasswordHelper
using LaTiendaApi.Models;
using LaTiendaApi.Services; // JwtService
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace LaTiendaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LatiendaContext _context;
        private readonly JwtService _jwtService; // Servicio encargado de generar token

        public AuthController(LatiendaContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            if (!PasswordHelper.VerifyPassword(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return Unauthorized("Contraseña incorrecta");

            var roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList();
            var token = _jwtService.GenerateToken(usuario, roles);

            return new LoginResponseDto
            {
                Token = token,
                Nombre = usuario.Nombre,
                Roles = roles
            };
        }
    

    // Agregar el endpoint en AuthController para registrar
    [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar(RegisterRequestDto request)
        {
            try
            {
                // Verificar si el correo ya está registrado
                if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                    return BadRequest(new { msj = "El email ya está registrado" });

                // Si no envían roles, asignar role 'User' por defecto
                if (request.Roles == null || !request.Roles.Any())
                {
                    var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "User");
                    if (userRole == null)
                        return BadRequest(new { msj = "No existe el role 'User' en la base de datos. Contacta al administrador." });
                    request.Roles = new List<int> { userRole.RolId };
                }

                // Comprobar que los roles existan
                var existingRoleIds = await _context.Roles.Where(r => request.Roles.Contains(r.RolId)).Select(r => r.RolId).ToListAsync();
                if (existingRoleIds.Count != request.Roles.Count)
                {
                    return BadRequest(new { msj = "Uno o varios roles no existen" });
                }

                // Crear hash y salt de la contraseña
                PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    TipoDoc = request.TipoDoc,
                    NroDoc = request.NroDoc,
                    Nombre = request.Nombre,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                // Asignar roles (sólo RolId)
                foreach (var rolId in request.Roles)
                {
                    usuario.UsuarioRoles.Add(new UsuarioRole
                    {
                        RolId = rolId
                    });
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok(new { msj = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {
                // Devolver detalle en entorno de desarrollo para depuración
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = "Error interno al registrar usuario", error = ex.Message });
            }
        }
    }
}
