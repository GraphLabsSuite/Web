using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class SurveyCreatingModel
    {
        public string question { get; set; }
        public List<KeyValuePair<string, bool>> questionOptions { get; set; }
    }
}