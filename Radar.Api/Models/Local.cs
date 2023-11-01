using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Radar.Api.Models;

[Table("Local")]
public class Local
{
    #region Column
    [Key]
    [Column("LocalID")]
    public int LocalId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Descricao { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Verificado { get; set; } = null!;
    #endregion Column

    #region Relationship
    [InverseProperty("Local")]
    [JsonIgnore]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    #endregion Relationship
}