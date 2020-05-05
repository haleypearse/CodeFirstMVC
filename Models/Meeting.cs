using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public DateTime Time { get; set; }
        public int? TimesMet { get; set; }
    }
}