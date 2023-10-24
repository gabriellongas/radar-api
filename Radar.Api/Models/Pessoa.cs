namespace Radar.Api.Models
{
    public class Pessoa
    {
        public int PessoaID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTimeOffset DataNascimento { get; set; }
    }
}
