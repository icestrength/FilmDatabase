using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace FilmDatabase.Models
{
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public FilmContext()
            : base("DefaultConnection1")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>().HasMany(c => c.Categories)
             .WithMany(s => s.Films)
             .Map(t => t.MapLeftKey("FilmId")
             .MapRightKey("CategoryId")
             .ToTable("CategoryFilm"));
        }
    }
}