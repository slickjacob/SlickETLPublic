using MailChimp.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlickETL.Web.Models
{
    [System.Runtime.Serialization.DataContract]
    public class MailChimpSignUpModel : MergeVar
    {
        [System.Runtime.Serialization.DataMember(Name = "FNAME")]
        public string FirstName { get; set; }
        [System.Runtime.Serialization.DataMember(Name = "LNAME")]
        public string LastName { get; set; }
        [System.Runtime.Serialization.DataMember(Name = "COMPANY")]
        public string Company { get; set; }
        [System.Runtime.Serialization.DataMember(Name = "HOOK")]
        public string Hook { get; set; }
    }
}