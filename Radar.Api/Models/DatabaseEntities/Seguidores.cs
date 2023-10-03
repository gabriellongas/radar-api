namespace Radar.Api.Models.DatabaseEntities
{
    public class Seguidores
    {
        public int SeguidorID { get; set; }
        public int PessoaIDSeguida { get; set; }
        public int PessoaIDSeguidor { get; set; }
        
    }
}