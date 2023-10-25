using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radar.Api.Models
{
    public class Post
    {
        [Key] public int PostID { get; set; }
        [ForeignKey("PessoaID")] public Pessoa Pessoa { get; set; } = new();
        [ForeignKey("LocalID)")] public Local Local { get; set; } = new();
        public string? Conteudo { get; set; }
        public int Avaliacao { get; set; }
        public DateTimeOffset DataPostagem { get; set; }
        public int Likes { get; set; }
    }
}
