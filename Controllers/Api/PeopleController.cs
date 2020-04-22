using CodeFirstMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;

namespace CodeFirstMVC.Controllers.Api
{
    public class PeopleController : ApiController
    {
        // DbSet<Person> People { get; set; }

        public ApplicationDbContext _context;

        public PeopleController()
        {
            _context = new ApplicationDbContext();
        }

        // Get/Api/People
        public IEnumerable<Person> GetPeople()
        {
            return new List<Person> { new Person { Name = "yin" }, new Person { Name = "yang" } };
            
            //return new string[] { "value 1", "value 2" };

        }
        //public class ApiContext : DbContext
        //{
        //    public ApiContext()// : base("MyContext")
        //    {
        //    }
            
        //}


        //private DbContext _context;
        //public PeopleController()
        //{
        //    _context = new DbContext();
        //}
        //// Get/Api/People
        //public IEnumerable<Person> GetPeople()
        //{
        //    return _context.People.ToList();
        //}
    }
}
 