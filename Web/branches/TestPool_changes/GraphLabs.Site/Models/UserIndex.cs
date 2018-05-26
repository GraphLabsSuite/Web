using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class UserIndex
    {
        public int Id { get; set; }

        public bool VerStudent { get; set; }

        public bool UnVerStudent { get; set; }

        public bool Teacher { get; set; }

        public bool Admin { get; set; }

        public bool DismissStudent { get; set; }

        public List<UserModel> Users { get; set;}

        public UserIndex()
        {
            VerStudent = false;
            UnVerStudent = true;
            Teacher = false;
            Admin = false;
            DismissStudent = false;
        }
    }
}