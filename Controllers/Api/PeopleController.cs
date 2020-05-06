using System; 
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
        //[HttpGet]
        //[ResponseType(typeof(IEnumerable<Person>))]

        //public IEnumerable<Person> GetPeople(string searchString=null)
        public IEnumerable<PersonDto> GetPeople(string searchString=null,string sort="name", bool asc=true)
        {
            var people = db.People.Where(x => true);
            if (searchString != null)
            {
                people = people.Where(x => x.Name.Contains(searchString) || x.TimesMet.ToString() == searchString);
            }
            switch (sort)
            {
                case "TimesMet":
                    if (asc == true) people = people.OrderBy(x => x.TimesMet);
                    else people = people.OrderByDescending(x => x.TimesMet);
                    break;
                case "WhenMet":
                    if (asc == true) people = people.OrderBy(x => x.WhenMet);
                    else people = people.OrderByDescending(x => x.WhenMet);
                    break;
                case "LastMet":
                    if (asc == true) people = people.OrderBy(x => x.LastMet);
                    else people = people.OrderByDescending(x => x.LastMet);
                    break;
                default:
                    if (asc == true) people = people.OrderBy(x => x.Name);
                    else people = people.OrderByDescending(x => x.Name);
                    break;
            }
            //return people;
            return people.ToList().Select(Mapper.Map<Person, PersonDto>);
        }

        // GET: api/People/5
        [HttpGet]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(string name)
        {
            // Find is apparently not case sensitive
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
            name = name.Transform(To.TitleCase); //This helps to match entries in the db, also capitalized
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
            var meetings = db.Meetings.Where(x => x.Person.Name.Contains(name));

            if (person == null)
            {
                return NotFound();
            }

            //foreach (Meeting meeting in meetings)
            //{
            //    db.Meetings.Remove(meeting);
            //}

            Person anonymousPerson = EnsureAnonymousPersonExists();

            //Change all meetings to anonymous
            foreach (Meeting meeting in meetings)
            {
                System.Diagnostics.Debug.WriteLine(meeting.Person.Name);
                meeting.Person = anonymousPerson;
                System.Diagnostics.Debug.WriteLine("after "+ meeting.Person.Name);


            }

            //db.Entry(person).State = EntityState.Modified;
            db.Entry(meetings).State = EntityState.Modified;

            //db.People.Remove(person);
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
            name = name.Transform(To.TitleCase); //This helps to match entries in the db, also capitalized
            return db.People.Count(e => e.Name == name) > 0;
        }

        private Person EnsureAnonymousPersonExists()
        {
            Person person = new Person();
            Person query = db.People.Find("Anonymous");

            if (query == null)
            {
                person.Name = "Anonymous";
                person.TimesMet = 0;
                db.People.Add(person);
                db.SaveChangesAsync();
                return person;
            }

            db.SaveChangesAsync();
            return query;
        }
    }
}