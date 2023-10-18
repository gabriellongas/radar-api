namespace Radar.Api.Data.Entities
{
    public class Seguidores
    {
        public int SeguidorID { get; set; }
        public int PessoaIDSeguida { get; set; }
        public int PessoaIDSeguidor { get; set; }

    }
}