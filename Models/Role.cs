
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LaTiendaApi.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
