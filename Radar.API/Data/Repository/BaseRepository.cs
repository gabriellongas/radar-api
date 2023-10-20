using Radar.Api.Data.Context;
using System.Data;

namespace Radar.Api.Data.Repository
{
    public abstract class BaseRepository
    {
        private bool _dispose;
        protected readonly RADARContext _context;

        public BaseRepository(RADARContext context)
        {
            _dispose = false;
            _context = context;
        }

        public virtual void Dispose()
        {
            if (!_dispose)
            {
                _dispose = true;
                _context.Dispose();
            }
        }

        protected void OpenConnection()
        {
            if(_context.Connection.State == ConnectionState.Closed)
            {
                _context.Connection.Open();
            }
        }

        protected void CloseConnection()
        {
            if (_context.Connection.State != ConnectionState.Closed)
            {
                _context.Connection.Close();
            }
        }
    }
}
