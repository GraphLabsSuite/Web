using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Модель расписания (списка) </summary>
    public class LabScheduleListModel : ListModelBase<LabScheduleModel>,
        IFilterableByDate<LabScheduleListModel, LabScheduleModel>, 
        IFilterableByUser<LabScheduleListModel, LabScheduleModel>,
        IFilterableByLabName<LabScheduleListModel, LabScheduleModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> _modelLoader;

        public LabScheduleListModel(
            IEntityQuery query,
            IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        private DateTime? _dateFrom;
        private DateTime? _dateTill;
        private string _user0;
        private string _user1;
        private string _user2;
        private string _labname;
        

        public LabScheduleListModel FilterByDate(DateTime? from, DateTime? till)
        {
            if (from.HasValue && till.HasValue && till < from)
                throw new ArgumentOutOfRangeException();

            _dateFrom = from?.Date;
            _dateTill = till?.Date.AddDays(1).AddMilliseconds(-1);

            return this;
        }

        public LabScheduleListModel FilterByUser(string user)
        {
            // разбиваем на имя - отчество - фамилию или на год - номер группы
            string[] _user = new string[3];
            _user[0] = "";
            _user[1] = "";
            _user[2] = "";
            if (!string.IsNullOrEmpty(user))
            {
                string[] _helpuser = user.Split(' ');
                for (int i = 0; i < _helpuser.Length; i++)
                {
                    _user[i] = _helpuser[i];
                }
            }
            _user0 = _user[0];
            _user1 = _user[1];
            _user2 = _user[2];
            return this;
        }

        public LabScheduleListModel FilterByLabName(string labname)
        {
            _labname = labname;
            return this;
        }

        protected override LabScheduleModel[] LoadItems()
        {
            return _query
                .OfEntities<AbstractLabSchedule>()
                .OrderBy(m => m.DateFrom)
                .ThenBy(m => m.DateTill)
                .Where(m =>
                   (!_dateFrom.HasValue || _dateFrom <= m.DateTill)
                && (!_dateTill.HasValue || _dateTill >= m.DateFrom))
                .Where(m => (((m is IndividualLabSchedule) && 
                ((string.IsNullOrEmpty(_user0)) || 
                (((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user0) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user1) 
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user2)) ||
                (((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user0) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user2)
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user1)) ||
                ((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user1) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user0)
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user2)) ||
                ((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user1) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user2)
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user0)) ||
                ((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user2) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user0)
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user1)) ||
                ((m as IndividualLabSchedule).Student.Name.ToLower().Contains(_user2) && (m as IndividualLabSchedule).Student.FatherName.ToLower().Contains(_user1)
                                                                                && (m as IndividualLabSchedule).Student.Surname.ToLower().Contains(_user0))))))
                ||                                                     
                ((m is GroupLabSchedule) && ((m as GroupLabSchedule).Group.Name.Contains(_user1) && (m as GroupLabSchedule).Group.Name.Contains(_user1) || (string.IsNullOrEmpty(_user0))))))     
                .Where(m => (m.LabWork.Name.Equals(_labname) || string.IsNullOrEmpty(_labname)))
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}