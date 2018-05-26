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

        public long ResultId { get; set; }

        public long? HasTestPool { get; set; }

        public long LabVariant { get; set; }

        public String Result { get; set; }

        public String Mark { get; set; }

        public String Status { get; set; }

        public StudentsResultModel(long id, string name, DateTime date, string variant, long resId, int? result,ExecutionStatus status, long labvariant, long? hasTestPool)
        {
            Id = id;
            Name = name;
            Date = date;
            Variant = variant;
            ResultId = resId;
            Result = result.ToString();
            SetStatus(status);
            SetMark(result);
            LabVariant = labvariant;
            HasTestPool = hasTestPool;
        }

        private void SetMark(int? result)
        {
            if (result == null)
            {
                Mark = "";
            }
            else if (result >= 90)
            {
                Mark = "5";
            }
            else if (result >= 70)
            {
                Mark = "4";
            }
            else if (result >= 60)
            {
                Mark = "3";
            }
            else Mark = "2";
        }

        private void SetStatus(ExecutionStatus status)
        {
            switch (status)
            {
                    case ExecutionStatus.Complete:
                    Status = "Завершено";
                    break;
                    case ExecutionStatus.Executing:
                    Status = "Выполняется";
                    break;
                    case ExecutionStatus.Interrupted:
                    Status = "Прервано";
                    break;
            }
        }
    }
}