namespace Radar.Api.Models.Dto;

public class LocalUpdateDto
{
    public int LocalId { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public string Verificado { get; set; } = null!;
}