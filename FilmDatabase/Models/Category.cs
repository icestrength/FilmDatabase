using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmDatabase.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Film> Films { get; set; }

        public Category()
        {
            Films = new List<Film>();
        }
    }
}