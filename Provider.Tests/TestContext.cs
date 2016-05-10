using iAnywhere.Data.SQLAnywhere;
using Microsoft.EntityFrameworkCore;

namespace Provider.Tests
{
    public partial class TestDBContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var fac = new SAConnectionStringBuilder();
            fac.DatabaseName = "test";
            fac.UserID = "dba";
            fac.Password = "sql";
            fac.Integrated = "true";
            var connectionString = fac.ToString();

            options.UseSqlAnywhere(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var blog = modelBuilder.Entity<Blog>();

            blog
                .Property(b => b.Url)
                .IsRequired();

            blog.Property(b => b.BlogId)
                .ValueGeneratedOnAdd()
                .IsRequired();

        }
    }
}
