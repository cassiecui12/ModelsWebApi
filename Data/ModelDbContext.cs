using Microsoft.EntityFrameworkCore;
using ModelsWebAPI.Models;

namespace ModelsWebAPI.Data
{
    public class ModelDbContext : DbContext, IModelDbContext
    {
        protected ModelDbContext() { }

        public ModelDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Model> Models { get; set; }

        public void MarkAsModified(IModel item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}