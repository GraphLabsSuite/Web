using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.DAL
{
    public class GraphContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> Sessions { get; set; }
        public DbSet<News> News { get; set; }

        protected override void  OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}