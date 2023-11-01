namespace Radar.Api.Models.Dto;

public class SeguidoresReadDto
{
    public int SeguidorId { get; set; }
    public Pessoa PessoaSeguida { get; set; } = null!;
    public Pessoa PessoaSeguidor { get; set; } = null!;
}
