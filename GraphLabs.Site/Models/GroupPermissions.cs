using System.Collections.Generic;

namespace GraphLabs.Site.Models
{
    public class GroupPermissions
    {
        public int id { get; set; }

        public long ID_Group { get; set; }

        public string Group_Name { get; set; }

        public List<PermChoose> Permissions { get; set; }
    }
}