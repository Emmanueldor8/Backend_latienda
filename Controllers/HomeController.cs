using Microsoft.AspNetCore.Mvc;

namespace LaTiendaApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new 
            { 
                msj = "La Tienda API - Online",
                version = "1.0",
                endpoints = new
                {
                    auth = new { login = "POST /api/auth/login", registrar = "POST /api/auth/registrar" },
                    productos = new { lista = "GET /api/producto/lista", obtener = "GET /api/producto/obtener/{id}" },
                    docs = "GET /swagger"
                }
            });
        }
    }
}
