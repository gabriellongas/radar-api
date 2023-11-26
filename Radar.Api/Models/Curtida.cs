using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radar.Api.Models;

[Table("Curtida")]
public class Curtida
{
    #region Column
    [Key]
    [Column("CurtidaID")]
    public int CurtidaId { get; set; }

    [Column("PessoaIDCurtindo")]
    public int PessoaIdCurtindo { get; set; }

    [Column("PostIDCurtido")]
    public int PostIdCurtido { get; set; }
    #endregion Column

    #region Relationship
    [ForeignKey("PessoaIdCurtindo")]
    [InverseProperty("Curtidas")]
    public virtual Pessoa PessoaIdCurtindoNavigation { get; set; } = null!;

    [ForeignKey("PostIdCurtido")]
    [InverseProperty("Curtidas")]
    public virtual Post PostIdCurtidoNavigation { get; set; } = null!;
    #endregion Relationship
}