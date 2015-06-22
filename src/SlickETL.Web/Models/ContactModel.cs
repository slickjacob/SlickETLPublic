using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlickETL.Web.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage="Please enter a valid email address!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Please enter a valid email address!")]
        public string Email { get; set; }
        [Required(ErrorMessage="Please enter a first name!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage="Please enter a last name!")]
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Hook { get; set; }
        public bool Subscribe { get; set; }
        public string Message { get; set; }
    }
}