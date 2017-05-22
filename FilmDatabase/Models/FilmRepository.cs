using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace FilmDatabase.Models
{
    public class FilmRepository : IFilmRepository
    {
        private FilmContext context = new FilmContext();
        public List<Film> Films
        {
            get { return context.Films.ToList(); }
        }
        public List<Category> Categories
        {
            get { return context.Categories.ToList(); }
        }
        public List<Comment> Comments
        {
            get { return context.Comments.ToList(); }
        }
        public void Add(Film f)
        {
            context.Films.Add(f);
        }
        public void AddComment(Comment c)
        {
            context.Comments.Add(c);
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public void AddCategory(Category c)
        {
            context.Categories.Add(c);
        }
        public void Remove(int id)
        {
            Film f = Films.Find(m => m.Id == id);
            context.Films.Remove(f);
            SaveChanges();
        }

        public void RemoveComment(int id)
        {
            Comment c = Comments.Find(m => m.Id == id);
            context.Comments.Remove(c);
            SaveChanges();
        }
        public Film GetFilm(int id)
        {
            Film f = Films.Find(m => m.Id == id);
            return f;
        }
        public List<Film> Include()
        {
            return context.Films.Include(p => p.Comments).Include(m=>m.Marks).ToList();
        }


        public List<Mark> Marks
        {
            get { return context.Marks.ToList(); }
        }

        public void AddMark(Mark m)
        {
            context.Marks.Add(m);
        }
    }
}