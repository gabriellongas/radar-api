using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radar.Api.Models;

[Table("Seguidores")]
public class Seguidor
{
    #region Column
    [Key]
    [Column("SeguidorID")]
    public int SeguidorId { get; set; }

    [Column("PessoaIDSeguida")]
    public int? PessoaIdseguida { get; set; }

    [Column("PessoaIDSeguidor")]
    public int? PessoaIdseguidor { get; set; }
    #endregion Column

    #region Relationship
    [ForeignKey("PessoaIdseguida")]
    [InverseProperty("SeguidoresPessoaIdSeguidaNavigation")]
    public virtual Pessoa? PessoaIdSeguidaNavigation { get; set; }

    [ForeignKey("PessoaIdseguidor")]
    [InverseProperty("SeguidoresPessoaIdSeguidorNavigation")]
    public virtual Pessoa? PessoaIdSeguidorNavigation { get; set; }
    #endregion Relationship
}
