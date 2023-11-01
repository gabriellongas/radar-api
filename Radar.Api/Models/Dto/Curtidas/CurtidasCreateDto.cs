namespace Radar.Api.Models.Dto;

public class CurtidasCreateDto
{
    public int CurtidaId { get; set; }
    public int PessoaIdCurtindo { get; set; }
    public int PostIdCurtido { get; set; }
}