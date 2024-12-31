using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
                {
                    var entity = modelBuilder.Entity(entityType.ClrType);
                    entity.Property<long>("Id").HasColumnName($"{entityType.ClrType.Name}Id");
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
