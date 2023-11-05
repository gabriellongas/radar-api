namespace Radar.Api.Models.Dto;

public class PessoaCreateDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataNascimento { get; set; }
}
