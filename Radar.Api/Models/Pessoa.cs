using System;
using System.Collections.Generic;

namespace Radar.Api.Models;

public partial class Pessoa
{
    public int PessoaId { get; set; }

    public string? Nome { get; set; }

    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Senha { get; set; }

    public string? Descricao { get; set; }

    public DateTime? DataNascimento { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Seguidore> SeguidorePessoaIdseguidaNavigations { get; set; } = new List<Seguidore>();

    public virtual ICollection<Seguidore> SeguidorePessoaIdseguidorNavigations { get; set; } = new List<Seguidore>();
}
