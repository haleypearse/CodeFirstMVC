using System; 
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using CodeFirstMVC.Dtos;
using CodeFirstMVC.Models;
using Humanizer;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Serilog;

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
        public IEnumerable<PersonDto> GetPeople([FromUri] PagingParameterModel pagingparametermodel)
        {

            var people = db.People.Where(x => true);

            var listy = people.ToList();

            // Get search query
            string query = pagingparametermodel.Query;

            // Get sort order
            string sortBy = pagingparametermodel.SortBy;
            
            // Get sort order
            bool sortOrder = pagingparametermodel.SortOrder;

            // Do filtering
            if (query != null) people = people.Where(x => x.Name.Contains(query) || x.TimesMet.ToString() == query);
            
            // Do sorting
            switch (sortBy)
            {
                case "TimesMet":
                    if (sortOrder == true) people = people.OrderBy(x => x.TimesMet);
                    else people = people.OrderByDescending(x => x.TimesMet);
                    break;
                case "WhenMet":
                    if (sortOrder == true) people = people.OrderBy(x => x.WhenMet);
                    else people = people.OrderByDescending(x => x.WhenMet);
                    break;
                case "LastMet":
                    if (sortOrder == true) people = people.OrderBy(x => x.LastMet);
                    else people = people.OrderByDescending(x => x.LastMet);
                    break;
                default:
                    if (sortOrder == true) people = people.OrderBy(x => x.Name);
                    else people = people.OrderByDescending(x => x.Name);
                    break;
            }

            // Get's No of Rows Count   
            int count = people.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var returnpeople = people.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            returnpeople.ForEach(x => x.HumanizedWhenMet = x.WhenMet.Humanize());
            returnpeople.ForEach(x => x.HumanizedLastMet = x.LastMet.Humanize());

            //var returnpeople = 
            //for (Person person in pagepeople)
            //{

            //}


            //    // = returnpeople.LastMet.Humanize();
            //Console.WriteLine(returnpeople).;


            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                Query = query,
                SortBy = sortBy,
                SortOrder = sortOrder,
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            //ViewBag.paginationMetadata = paginationMetadata;
            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            //HttpContext.Current.Response.Headers.Add()

            //return people;
            return returnpeople.ToList().Select(Mapper.Map<Person, PersonDto>);
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

            //Person anonymousPerson = EnsureAnonymousPersonExists();

            Log.Information("Deleting person: {person} named {person.Name}, and setting their meetings {meetings} to null.", person, person.Name, meetings);

            //Change all meetings.Person to null
            foreach (Meeting meeting in meetings) meeting.Person = null;

            //System.Diagnostics.Debug.WriteLine("before " + meeting.Person.Name);
            //db.Meetings.Find(meeting.Id) = null;

            //System.Diagnostics.Debug.WriteLine("after " + meeting.Person);


        
            //db.Meetings = (DbSet<Meeting>)meetings;
            //db.Entry(person).State = EntityState.Modified;
            // db.Entry(db.Meetings).State = EntityState.Modified;

            db.People.Remove(person);
            db.SaveChanges();
            

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