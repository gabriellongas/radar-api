using System;
using System.Collections.Generic;

namespace Radar.Api.Models;

public partial class Local
{
    public int LocalId { get; set; }

    public string? Nome { get; set; }

    public string? Descricao { get; set; }

    public string? Endereco { get; set; }

    public string? Verificado { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
