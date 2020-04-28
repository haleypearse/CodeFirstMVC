﻿using System; 
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using CodeFirstMVC.Dtos;
using CodeFirstMVC.Models;
using Humanizer;
using Microsoft.Ajax.Utilities;

namespace CodeFirstMVC.Controllers.Api
{
    public class PeopleController : ApiController
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private MyContext db = new MyContext();

        // GET: api/People
        [HttpGet]
        public IEnumerable<PersonDto> GetPeople(string searchString)
        {
            var people = db.People.Where(x => true);
            if (searchString != null)
            {
                //var query = 
                //    from p in people
                //    where p.Name contains searchString
                //    select p;

                people = people.Where(x => x.Name.Contains(searchString) || x.TimesMet.ToString() == searchString);
            }
            // Try humanizing the dates. Doesn't work, can't convert date to string.
            //var people = db.People;
            //foreach (var person in people)
            //{
            //    person.WhenMet = person.WhenMet.Humanize();
            //}
            //return people;
            return people.ToList().Select(Mapper.Map<Person, PersonDto>);
        }

        // GET: api/People/5
        [HttpGet]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(string name)
        {
            Person person = await db.People.FindAsync(name);
            if (person == null)
            {
                return NotFound();
            }
            //return Ok(person);
            return Ok(Mapper.Map<Person, PersonDto>(person));
        }

        // PUT: api/People/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(string name, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (name != person.Name)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/People
        [HttpPost]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(person);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(person.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { name = person.Name }, person);
        }

        // DELETE: api/People/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(string name)
        {
            Person person = await db.People.FindAsync(name);
            if (person == null)
            {
                return NotFound();
            }

            db.People.Remove(person);
            await db.SaveChangesAsync();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(string name)
        {
            return db.People.Count(e => e.Name == name) > 0;
        }
    }
}