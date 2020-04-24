using AutoMapper;
using CodeFirstMVC.Dtos;
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
    public class People2Controller : ApiController
    {
        // DbSet<Person> People { get; set; }

        // should be private
        public ApplicationDbContext _context;

        public People2Controller()
        {
            _context = new ApplicationDbContext(); // Created my own class because no auth scaffolded
        }

        // Get/Api/People
        public IEnumerable<PersonDto> GetPeople()
        {
            return _context.People.ToList().Select(Mapper.Map<Person, PersonDto>);
                
                //Select(Mapper.Map<Costomer, CustomerDto>);

            //return new List<Person> { new Person { Name = "yin" }, new Person { Name = "yang" } };



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
 