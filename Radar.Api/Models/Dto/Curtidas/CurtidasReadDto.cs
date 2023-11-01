namespace Radar.Api.Models.Dto;

public class CurtidasReadDto
{
    public int CurtidaId { get; set; }
    public Pessoa PessoaIdCurtindo { get; set; } = null!;
    public Post PostIdCurtido { get; set; } = null!;
}