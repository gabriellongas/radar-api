namespace Radar.Api.Models.DatabaseEntities
{
    public class Post
    {
        public int PostID { get; set; }
        public int PessoaID { get; set; }
        public int LocalID { get; set; }
        public string? Conteudo { get; set; }
        public int Avaliacao { get; set; }
        public DateTime DataPostagen { get; set; }
        public int Likes { get; set; }
    }
}