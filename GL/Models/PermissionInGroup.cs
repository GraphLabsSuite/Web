using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GL.Models
{
    public class PermissionInGroup
    {
        public long ID { get; set; }

        public long ID_Permission { get; set; }

        public bool Enable { get; set; }

        public string Name { get; set; }
    }
}