using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDatabase.Models
{
    public interface IFilmRepository
    {
        List<Film> Films { get; }
        List<Category> Categories { get; }
        List<Comment> Comments { get; }
        List<Mark> Marks { get; }
        void Add(Film f);
        Film GetFilm(int id);
        void Remove(int id);
        void RemoveComment(int id);
        void SaveChanges();
        void AddCategory(Category c);
        void AddComment(Comment c);
        void AddMark(Mark m);
        List<Film> Include();
    }
}
