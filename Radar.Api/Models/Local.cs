namespace Radar.Api.Models
{
    public class Local
    {
        public int LocalID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string? Verificado { get; set; }
    }
}
