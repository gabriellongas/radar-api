using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Radar.Api.Models.Dto;

public class PostReadDto
{
    public int PostId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;
    public Local Local { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public int Avaliacao { get; set; }
    public DateTime DataPostagem { get; set; }
    public int Curtidas { get; set; }
}
