using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickETL.Tests
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void gmail_can_send()
        {
            var c = new SlickETL.Web.Models.ContactModel() { FirstName = "Jake", LastName = "Snake", Message = "This is a test", Email = "jake@snake.com", Company = "mine", Subscribe = true };
            SlickETL.Web.Utility.Email.SendGmail(c);

        }
        [Test]
        public void sendgrid_can_send()
        {
            var c = new SlickETL.Web.Models.ContactModel() { FirstName = "Jake", LastName = "Snake", Message = "This is a test", Email = "jake@snake.com", Company = "mine", Subscribe = true };
            SlickETL.Web.Utility.Email.SendGridEmail(c);
        }
    }
}
