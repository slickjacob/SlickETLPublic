using MailChimp;
using MailChimp.Helper;
using SlickETL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlickETL.Web.Utility
{
    public class MailChimp
    {
        public static void Signup(string EmailAddress, string FirstName, string LastName, string Company, string Hook)
        {
            var mailChimpListID = "c048a56a60";
            var email = new EmailParameter() { Email = EmailAddress };
            var mc = new MailChimpSignUpModel();
            mc.FirstName = FirstName;
            mc.LastName = LastName;
            mc.Company = Company;
            mc.Hook = Hook;
            var mcm = new MailChimpManager("e60fe47c1d4845c1292f6f0352946403-us11");
            EmailParameter results = mcm.Subscribe(mailChimpListID, email, mc, updateExisting: true, doubleOptIn: false);
        }
    }
}