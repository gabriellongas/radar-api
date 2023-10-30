using System;
using System.Collections.Generic;

namespace Radar.Api.Models;

public partial class Seguidore
{
    public int SeguidorId { get; set; }

    public int? PessoaIdseguida { get; set; }

    public int? PessoaIdseguidor { get; set; }

    public virtual Pessoa? PessoaIdseguidaNavigation { get; set; }

    public virtual Pessoa? PessoaIdseguidorNavigation { get; set; }
}
