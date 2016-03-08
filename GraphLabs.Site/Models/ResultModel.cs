using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;

namespace GraphLabs.Site.Models
{
    public class ResultModel
    {
        public GroupModel[] Groups { get; set; }

        public LabWork[] Labs { get; set; }
    }
}