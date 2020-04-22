using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Dtos
{
    public class PersonDto
    {
        [Key]
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; }

        public string LuckyNumber { get; set; }
        public int TimesMet { get; set; }

        public DateTime WhenMet { get; set; }

        public DateTime LastMet { get; set; }
    }
}