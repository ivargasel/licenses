using Microsoft.EntityFrameworkCore;
using Modules.API.Model;

namespace Modules.API.Persistence
{
    public class ModuleContext (DbContextOptions options) : DbContext(options)
    {
        public DbSet<ModuleModel> Modules { get; set; }
    }
}