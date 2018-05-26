using System;
using System.Linq;
using System.Linq.Expressions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> Базовая модель списка ДР, доступных для выполнения </summary>
    public abstract class AvailableLabListModel<TAvalilableLab> : ListModelBase<TAvalilableLab>
        where TAvalilableLab : AvailableLabModel
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<TAvalilableLab, AbstractLabSchedule> _modelLoader;
        private readonly ISystemDateService _dateService;
        private readonly IGraphLabsPrincipal _currentUser;

        /// <summary> Модель списка демонстрационных лабораторных работ </summary>
        protected AvailableLabListModel(IEntityQuery query, 
            IEntityBasedModelLoader<TAvalilableLab, AbstractLabSchedule> modelLoader, 
            ISystemDateService dateService,
            IGraphLabsPrincipal currentUser)
        {
            _query = query;
            _modelLoader = modelLoader;
            _dateService = dateService;
            _currentUser = currentUser;
        }

        /// <summary> Режим выполнения </summary>
        protected abstract LabExecutionMode ExecutionMode { get; }

        /// <summary> Загружает демонстрационные лабораторные работы </summary>
        protected override TAvalilableLab[] LoadItems()
        {
            var currentStudent = _query.OfEntities<Student>().SingleOrDefault(s => s.Email == _currentUser.Identity.Name);
            if (currentStudent == null)
                throw new GraphLabsException("Данная страница имеет смысл только для залогиненных студентов.");

            var currentTime = _dateService.Now();
            var models = _query.OfEntities<IndividualLabSchedule>()
                .Where(s => s.Student.Id == currentStudent.Id)
                .Cast<AbstractLabSchedule>()
                .Union(_query.OfEntities<GroupLabSchedule>().Where(g => g.Group.Id == currentStudent.Group.Id))
                .Where(sch => sch.Mode == ExecutionMode 
                              && sch.DateFrom <= currentTime && sch.DateTill >= currentTime)
                .Where(GetAdditionalScheduleFilter(_query, currentStudent))
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .ToArray();

            return models;
        }

        /// <summary> Дополнительное условие фильтрации </summary>
        protected virtual Expression<Func<AbstractLabSchedule, bool>> GetAdditionalScheduleFilter(IEntityQuery query, Student student)
        {
            return s => true;
        }
    }
}