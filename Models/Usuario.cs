using System;
using System.Collections.Generic;

namespace LaTiendaApi.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string TipoDoc { get; set; } = null!;

    public string NroDoc { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}

