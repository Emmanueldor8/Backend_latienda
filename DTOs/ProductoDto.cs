using System.ComponentModel.DataAnnotations;

namespace LaTiendaApi.DTOs
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
        public bool? Estado { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }

    public class ProductoCreateDto
    {
        [Required]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public bool Estado { get; set; }
    }

    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}


