using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Models
{
    public class Person
    {
        //[DatabaseGenerated]
        //public int PersonId { get; set; }
        [Key]
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; }
        [Display(Name="Times Met")]
        public int TimesMet { get; set; }
        [Display(Name = "When Met")]

        public Nullable<DateTime> WhenMet { get; set; }
        [Display(Name = "Last Met")]

        public Nullable<DateTime> LastMet { get; set; }
        public string HumanizedWhenMet { get; set; }
        public string HumanizedLastMet { get; set; }


    }
}