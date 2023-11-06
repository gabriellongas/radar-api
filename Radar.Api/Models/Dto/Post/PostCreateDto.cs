namespace Radar.Api.Models.Dto;

public class PostCreateDto
{
    public int PessoaId { get; set; }
    public int LocalId { get; set; }
    public string Conteudo { get; set; } = null!;
    public int Avaliacao { get; set; }
    public DateTime DataPostagem { get; set; }
}
