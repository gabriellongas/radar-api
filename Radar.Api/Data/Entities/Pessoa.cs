namespace Radar.Api.Data.Entities
{
    public class Pessoa
    {
        public int PessoaID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? Senha { get; set; }
        public DateOnly DataNascimento { get; set; }
    }
}