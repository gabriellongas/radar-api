using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Radar.Api.Models;

[Table("Pessoa")]
[Index("Login", Name = "UQ__Pessoa__5E55825BD0831999", IsUnique = true)]
public class Pessoa
{
    #region Column
    [Key]
    [Column("PessoaID")]
    public int PessoaId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Login { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [Column(TypeName = "date")]
    public DateTime DataNascimento { get; set; }
    #endregion Column

    #region Relationship
    [InverseProperty("PessoaIdCurtindoNavigation")]
    [JsonIgnore]
    public virtual ICollection<Curtidas> Curtidas { get; set; } = new List<Curtidas>();

    [InverseProperty("Pessoa")]
    [JsonIgnore]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("PessoaIdSeguidaNavigation")]
    [JsonIgnore]
    public virtual ICollection<Seguidores> SeguidoresPessoaIdSeguidaNavigation { get; set; } = new List<Seguidores>();

    [InverseProperty("PessoaIdSeguidorNavigation")]
    [JsonIgnore]
    public virtual ICollection<Seguidores> SeguidoresPessoaIdSeguidorNavigation { get; set; } = new List<Seguidores>();
    #endregion Relationship
}
