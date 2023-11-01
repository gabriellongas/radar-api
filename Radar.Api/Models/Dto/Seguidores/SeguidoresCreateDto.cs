namespace Radar.Api.Models.Dto;

public class SeguidoresCreateDto
{
    public int SeguidorId { get; set; }
    public int PessoaIdSeguida { get; set; }
    public int PessoaIdSeguidor { get; set; }
}
