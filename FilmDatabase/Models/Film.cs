using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmDatabase.Models
{
    public class Film
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public ICollection<Comment> Comments {get;set;}

        public virtual ICollection<Category> Categories { get; set; }
        public ICollection<Mark> Marks { get; set; }
        public Film()
        {
            Categories = new List<Category>();
            Comments = new List<Comment>();
            Marks = new List<Mark>();
        }
    }
}