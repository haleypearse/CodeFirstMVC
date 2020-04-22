using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Models
{
    public class MyContext: DbContext
    {
        public MyContext()// : base("MyContext")
        { }
        public DbSet<Person> People { get; set; }
    }
}