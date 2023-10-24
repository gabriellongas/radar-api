using Microsoft.EntityFrameworkCore;
using Radar.Api.Models;

namespace Radar.Api.Models
{
    public class RadarContext : DbContext
    {
        public RadarContext(DbContextOptions<RadarContext> options) : base(options) { }

        public DbSet<Post> Post { get; set; }

        public DbSet<Pessoa> Pessoa { get; set; }

        public DbSet<Local> Local { get; set; }
    }
}
