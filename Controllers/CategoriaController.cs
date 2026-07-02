using LaTiendaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaTiendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        public readonly LatiendaContext dbContext;

        public CategoriaController(LatiendaContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var lista = dbContext.Categorias
                    .OrderBy(c => c.Nombre)
                    .ToList();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idCategoria:int}")]
        public IActionResult Obtener(int idCategoria)
        {
            try
            {
                var oCategoria = dbContext.Categorias.Find(idCategoria);

                if (oCategoria == null)
                    return NotFound(new { msj = "Categoría no encontrada" });

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = oCategoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Categoria objeto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { msj = "Datos inválidos", errors = ModelState });
                }

                var existe = dbContext.Categorias.Any(c => c.Nombre.ToLower() == objeto.Nombre.ToLower());
                if (existe)
                {
                    return Conflict(new { msj = "Ya existe una categoría con ese nombre" });
                }

                dbContext.Categorias.Add(objeto);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { msj = "ok", response = objeto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar/{idCategoria:int}")]
        public IActionResult Editar(int idCategoria, [FromBody] Categoria objetoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { msj = "Datos inválidos", errors = ModelState });
                }

                var oCategoria = dbContext.Categorias.Find(idCategoria);

                if (oCategoria == null)
                    return NotFound(new { msj = "Categoría no encontrada" });

                var existeNombre = dbContext.Categorias.Any(c => c.IdCategoria != idCategoria && c.Nombre.ToLower() == objetoDto.Nombre.ToLower());
                if (existeNombre)
                {
                    return Conflict(new { msj = "Ya existe otra categoría con ese nombre" });
                }

                oCategoria.Nombre = objetoDto.Nombre;
                oCategoria.Estado = objetoDto.Estado;

                dbContext.Entry(oCategoria).State = EntityState.Modified;
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idCategoria:int}")]
        public IActionResult Eliminar(int idCategoria)
        {
            try
            {
                var oCategoria = dbContext.Categorias.Find(idCategoria);

                if (oCategoria == null)
                    return NotFound(new { msj = "Categoría no encontrada" });

                dbContext.Categorias.Remove(oCategoria);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }
    }
}