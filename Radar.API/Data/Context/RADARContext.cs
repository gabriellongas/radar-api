using System.Data;
using System.Data.SqlClient;

namespace Radar.Api.Data.Context
{
    public sealed class RADARContext : IDisposable
    {
        public IDbConnection Connection { get; private set; }
        public RADARContext(string connString)
        {
            this.Connection = new SqlConnection(connString);
        }

        public void Dispose()
        {
            if (Connection != null) 
                Connection.Dispose();
        }
    }
}
