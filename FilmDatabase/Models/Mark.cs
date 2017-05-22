using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmDatabase.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MarkValue { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
    }
}