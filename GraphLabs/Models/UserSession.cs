using System;

namespace GraphLabs.Site.Models
{
    public class UserSession
    {
        public int UserSessionID { get; set; }

        public int UserID { get; set; }

        public DateTime TimeStart { get; set; }

        public Boolean IsActive { get; set; }
    }
}