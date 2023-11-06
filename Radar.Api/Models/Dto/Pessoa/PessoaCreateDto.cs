using System.ComponentModel.DataAnnotations;

namespace Radar.Api.Models.Dto;

public class PessoaCreateDto
{
    [Required]
    public string Nome { get; set; } = null!;
    
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Login { get; set; } = null!;

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Senha { get; set; } = null!;
    public string? Descricao { get; set; }
    
    [Required]
    public DateTime DataNascimento { get; set; }
}
