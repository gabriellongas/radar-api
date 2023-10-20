using Dapper;
using Radar.Api.Data.Context;
using Radar.Api.Data.Entities;
using System.Text;

namespace Radar.Api.Data.Repository
{
    public class PessoaRepository : BaseRepository, IPessoaRepository
    {
        public PessoaRepository(RADARContext context) : base(context)
        {
        }

        public IEnumerable<Pessoa> ListarPessoas()
        {
            string query = "select * from Pessoa;";

            var result = _context.Connection.Query<Pessoa>(query);
            return result;
        }
    }
}
