using Radar.Api.Data.Entities;

namespace Radar.Api.Data
{
    public interface IPessoaRepository
    {
        IEnumerable<Pessoa> ListarPessoas();
    }
}
