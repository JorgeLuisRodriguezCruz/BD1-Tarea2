using Microsoft.EntityFrameworkCore;
using Tarea__2.Models;

namespace Tarea__2.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<ArticuloEntity> Articulo { get; set; }
        public DbSet<ClaseArticuloEntity> ClaseArticulo { get; set; }
        public DbSet<UserEntity> Usuario { get; set; }

    }
}
