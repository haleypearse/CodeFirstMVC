using CodeFirstMVC.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace CodeFirstMVC.Controllers.Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()// : base("DefaultConnection")
        {
            Debug.Write(Database.Connection.ConnectionString + " <=== from ApplicationDbContext ");

            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s + " <=== ApplicationDbContext this.Database.Log ");
        }

        //Do I need this???
        public DbSet<Person> People { get; set; }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        //PM>   Get-Service | Where-Object {$_.Name -like '*SQL*'}
    }
}