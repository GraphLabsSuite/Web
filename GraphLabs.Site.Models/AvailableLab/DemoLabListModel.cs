using System.Linq;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> Модель списка демонстрационных лабораторных работ </summary>
    public class DemoLabListModel : ListModelBase<DemoLabModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<DemoLabModel, AbstractLabSchedule> _modelLoader;
        private readonly ISystemDateService _dateService;
        private readonly IGraphLabsPrincipal _currentUser;

        /// <summary> Модель списка демонстрационных лабораторных работ </summary>
        public DemoLabListModel(IEntityQuery query, 
            IEntityBasedModelLoader<DemoLabModel, AbstractLabSchedule> modelLoader, 
            ISystemDateService dateService,
            IGraphLabsPrincipal currentUser)
        {
            _query = query;
            _modelLoader = modelLoader;
            _dateService = dateService;
            _currentUser = currentUser;
        }

        /// <summary> Загружает демонстрационные лабораторные работы </summary>
        protected override DemoLabModel[] LoadItems()
        {
            var currentStudent = _query.OfEntities<Student>().SingleOrDefault(s => s.Email == _currentUser.Identity.Name);
            if (currentStudent == null)
                throw new GraphLabsException("Данная страница имеет смысл только для залогиненных студентов.");

            var currentTime = _dateService.Now();
            var models = _query.OfEntities<IndividualLabSchedule>()
                .Where(s => s.Student.Id == currentStudent.Id)
                .Cast<AbstractLabSchedule>()
                .Union(_query.OfEntities<GroupLabSchedule>().Where(g => g.Group.Id == currentStudent.Group.Id))
                .Where(sch => sch.Mode == LabExecutionMode.IntroductoryMode 
                           && sch.DateFrom <= currentTime && sch.DateTill >= currentTime)
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .ToArray();

            return models;
        }
    }
}       