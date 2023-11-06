using System.ComponentModel.DataAnnotations;

namespace Radar.Api.Models.Dto
{
    public class PessoaLoginDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Senha { get; set; } = null!;
    }
}
