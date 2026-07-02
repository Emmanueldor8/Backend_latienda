using AutoMapper;
using LaTiendaApi.Models;
using LaTiendaApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaTiendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly LatiendaContext dbContext;
        private readonly IMapper _mapper;

        public ProductoController(LatiendaContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Buscar")]
        public IActionResult Buscar([FromQuery] string? nombre, [FromQuery] int? categoriaId)
        {
            try
            {
                var query = dbContext.Productos
                    .Include(c => c.objCategoria)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    var term = nombre.Trim().ToLower();
                    query = query.Where(p => p.Nombre != null && p.Nombre.ToLower().Contains(term));
                }

                if (categoriaId.HasValue)
                {
                    query = query.Where(p => p.IdCategoria == categoriaId.Value);
                }

                var resultados = query.Take(200).ToList();
                var dto = _mapper.Map<List<ProductoDto>>(resultados);

                return Ok(new { msj = "ok", response = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var lista = dbContext.Productos
                    .Include(c => c.objCategoria)
                    .ToList();

                var listaDto = _mapper.Map<List<ProductoDto>>(lista);

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = listaDto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            try
            {
                var oProducto = dbContext.Productos
                    .Include(c => c.objCategoria)
                    .FirstOrDefault(p => p.IdProducto == idProducto);

                if (oProducto == null)
                    return NotFound(new { msj = "Producto no encontrado" });

                var dto = _mapper.Map<ProductoDto>(oProducto);

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });

            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] ProductoCreateDto objetoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { msj = "Datos inválidos", errors = ModelState });
                }

                // Validar unicidad por nombre (case insensitive)
                if (!string.IsNullOrWhiteSpace(objetoDto.Nombre))
                {
                    var existe = dbContext.Productos.Any(p => p.Nombre != null && p.Nombre.ToLower() == objetoDto.Nombre.ToLower());
                    if (existe)
                    {
                        return Conflict(new { msj = "Ya existe un producto con ese nombre" });
                    }
                }

                Producto objeto = _mapper.Map<Producto>(objetoDto);
                dbContext.Productos.Add(objeto);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { msj = "ok", response = objeto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar/{idProducto:int}")]
        public IActionResult Editar(int idProducto, [FromBody] ProductoCreateDto objetoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { msj = "Datos inválidos", errors = ModelState });
                }

                // Validar unicidad por nombre excluyendo el mismo registro
                var existeNombre = dbContext.Productos.Any(p => p.IdProducto != idProducto && p.Nombre != null && p.Nombre.ToLower() == objetoDto.Nombre.ToLower());
                if (existeNombre)
                {
                    return Conflict(new { msj = "Ya existe otro producto con ese nombre" });
                }

                var oProducto = dbContext.Productos.Find(idProducto);

                if (oProducto == null)
                    return NotFound(new { msj = "Producto no encontrado" });

                _mapper.Map(objetoDto, oProducto);

                dbContext.Entry(oProducto).State = EntityState.Modified;
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                var oProducto = dbContext.Productos.Find(idProducto);

                if (oProducto == null)
                    return NotFound(new { msj = "Producto no encontrado" });

                dbContext.Productos.Remove(oProducto);
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