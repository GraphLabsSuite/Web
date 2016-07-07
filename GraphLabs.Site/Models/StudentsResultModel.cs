using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using Microsoft.Ajax.Utilities;

namespace GraphLabs.Site.Models
{
    public class StudentsResultModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Variant { get; set; }

        public int Result { get; set; }

        public int Mark { get; set; }

        public StudentsResultModel(long id, string name, DateTime date, string variant, int result)
        {
            Id = id;
            Name = name;
            Date = date;
            Variant = variant;
            Result = result;
            SetMark(result);
        }

        private void SetMark(int result)
        {
            if (result >= 90)
            {
                Mark = 5;
            }
            else if (result >= 70)
            {
                Mark = 4;
            }
            else if (result >= 60)
            {
                Mark = 3;
            }
            else Mark = 2;
        }
    }
}