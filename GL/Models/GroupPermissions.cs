using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;using GL.Models;

namespace GL.Models
{
    public class GroupPermissions
    {
        public int id { get; set; }

        public long ID_Group { get; set; }

        public string Group_Name { get; set; }

        public List<PermChoose> Permissions { get; set; }
    }
}