using System.ComponentModel.DataAnnotations;

namespace Radar.Api.Models.Dto
{
    public class UpdatePasswordDto
    {
        [Required]
        public int PessoaId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string NewPassword { get; set; } = null!;
    }
}
