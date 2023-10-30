using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radar.Api.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? PessoaId { get; set; }

    public int? LocalId { get; set; }

    public string? Conteudo { get; set; }

    public int? Avaliacao { get; set; }

    public DateTime? DataPostagem { get; set; }

    public int? Likes { get; set; }

    public virtual Local? Local { get; set; }

    public virtual Pessoa? Pessoa { get; set; }
}
