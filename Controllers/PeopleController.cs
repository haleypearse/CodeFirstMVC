using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeFirstMVC.Models;
using Humanizer;
using Serilog;

namespace CodeFirstMVC
{
    public class PeopleController : Controller
    {
        private MyContext db = new MyContext();

        // GET: People
        public ActionResult Index()
        {
            //var x = db.Database.Connection.ConnectionString;

            //return View(db.People.ToList());
            return View();  //Getting list from Api now. No need to return it.
        }

        // GET: People/Details/5
        public ActionResult Details(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,TimesMet,WhenMet")] Person person)
        {
            var query = db.People.Find(person.Name);

            
            if (ModelState.IsValid)
            {
                if (query == null) // person not in db
                {
                    person.TimesMet = 1;
                    person.WhenMet = DateTime.UtcNow;
                    person.Name = person.Name.Transform(To.TitleCase); //Capitalize name
                    //db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.People ON");
                    db.People.Add(person);
                    Log.Information("Created a new person: {person}", person.Name);

                }
                else {
                    person = query;
                    person.TimesMet += 1;
                    Log.Information("Met {person} {times} times now.", person.Name, person.TimesMet+1); 
                }
                ViewBag.LastMet = person.LastMet.Humanize();

                person.LastMet = DateTime.UtcNow;
                db.SaveChanges();
                ViewBag.TimesMet = person.TimesMet.ToOrdinalWords(); 


                // return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,Name,TimesMet,WhenMet")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
