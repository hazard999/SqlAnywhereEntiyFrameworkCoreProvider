using iAnywhere.Data.SQLAnywhere;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Provider.Tests
{
    public partial class TestDBContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlAnywhere(GetConnectionString());
            options.EnableSensitiveDataLogging();     
        }
        
        public DbCommand GetDBCommand()
        {
            var conn = new SAConnection(GetConnectionString());
            conn.Open();

            return conn.CreateCommand();
        }             

        private string GetConnectionString()
        {
            var fac = new SAConnectionStringBuilder();
            fac.ServerName = "test";
            fac.UserID = "dba";
            fac.Password = "sql";
            fac.Integrated = "true";
            return fac.ToString();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var blog = modelBuilder.Entity<Blog>();

            blog
                .Property(b => b.Url)
                .HasMaxLength(500)
                .IsRequired();

            blog.Property(b => b.BlogId);
                //.ValueGeneratedOnAdd()
                //.IsRequired();

            blog.HasKey(f => f.BlogId);
            blog.HasIndex(f => f.BlogId);
            
        }
    }
}
