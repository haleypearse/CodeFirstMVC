using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Models
{
    public class MyContext: DbContext
    {
        public MyContext()// : base("MyContext")
        {
            Debug.Write(Database.Connection.ConnectionString + " <=== from MyContext ");

            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s + " <=== MyContext Database.Log ");
        }
        public DbSet<Person> People { get; set; }
    }
}