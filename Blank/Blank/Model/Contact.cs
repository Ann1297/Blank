using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blank
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Country Country { get; set; }
        public City City { get; set; }
        public University University { get; set; }
    }
}
