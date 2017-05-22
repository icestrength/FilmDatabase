using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace FilmDatabase.Models
{
    public class FilmDbInit : DropCreateDatabaseAlways<FilmContext>
    {
        protected override void Seed(FilmContext context)
        {
            Film one = new Film { Id = 1, Name = "First", Description = "blablabla" };
            Film two = new Film { Id = 2, Name = "First11", Description = "blablabla111" };
            context.Films.Add(one);
            context.Films.Add(two);
            Category c1 = new Category { Id = 1, Name = "Thriller", Films = new List<Film>() { one, two } };
            Category c2 = new Category { Id = 2, Name = "Horror", Films = new List<Film>() { two } };
            Category c3 = new Category { Id = 3, Name = "Comedy" };
            context.Categories.Add(c1);
            context.Categories.Add(c2);
            context.Categories.Add(c3);
            
            base.Seed(context);
        }
    }
}