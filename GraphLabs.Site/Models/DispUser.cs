using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class DispUser
    {
        public long ID { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string FatherName { get; set; }

        public string Email { get; set; }

        public string Group { get; set; }

        public bool Verify { get; set; }
    }
}