using Microsoft.EntityFrameworkCore;
using Permises.API.Model;

namespace Permises.API.Persistence
{
    public class PermiseContext (DbContextOptions options) : DbContext(options)
    {
        public DbSet<PermiseModel> Permises { get; set; }
    }
}
