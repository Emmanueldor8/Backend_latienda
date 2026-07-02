using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LaTiendaApi.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
