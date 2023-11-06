using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Radar.Api.Models;

[Table("Post")]
public class Post
{
    #region Column
    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Column("PessoaID")]
    public int PessoaId { get; set; }

    [Column("LocalID")]
    public int LocalId { get; set; }

    [Column(TypeName = "text")]
    public string Conteudo { get; set; } = null!;

    public int Avaliacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataPostagem { get; set; }

    public int Likes { get; set; }
    #endregion Column

    #region Relationship
    [InverseProperty("PostIdCurtidoNavigation")]
    [JsonIgnore]
    public virtual ICollection<Curtida> Curtidas { get; set; } = new List<Curtida>();

    [ForeignKey("LocalId")]
    [InverseProperty("Posts")]
    public virtual Local Local { get; set; } = null!;

    [ForeignKey("PessoaId")]
    [InverseProperty("Posts")]
    public virtual Pessoa Pessoa { get; set; } = null!;
    #endregion Relationship
}
