using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FilmDatabase.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage="Введіть коментар")]
        public string Body { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
    }
}