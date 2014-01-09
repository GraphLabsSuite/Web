using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;

namespace GraphLabs.Site.Models
{
    public class GroupModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<Student> Students { get; set; }

        public int FirstYear { get; set; }

        public int Number { get; set; }

        public GroupModel()
        {
        }

        public GroupModel(Group group, GraphLabsContext ctx)
        {
            Contract.Requires(group != null);

            Id = group.Id;
            IsOpen = group.IsOpen;
            Students = group.Students;
            FirstYear = group.FirstYear;
            Number = group.Number;
            var ds = new SystemDateService(ctx);
            Name = group.GetName(ds);
            //Name = 'K' + SemestrNumber(group.FirstYear) + '-' + group.Number.ToString();
        }

        private string SemestrNumber(int year)
        {
            DateTime curDate = DateTime.Today;
            
            int semestr = 1 + 2*(curDate.Year - year);
            //январь относится к семестру предыдущего года
            if (curDate.Month < 2)
            {
                semestr -= 2;
            }
            // с февраля по август - четный семестр
            if (curDate.Month > 1 && curDate.Month < 9)
            {
                semestr -= 1;
            }

            if (semestr < 10)
            {
                return '0' + semestr.ToString();
            }
            else
            {
                return semestr.ToString();
            }
        }
    }
}