using System;
using System.Linq;
using System.Linq.Expressions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> Загрузчик списка <see cref="TestingLabModel"/> </summary>
    public class TestingLabListModel : AvailableLabListModel<TestingLabModel>
    {
        /// <summary> Загрузчик списка <see cref="TestingLabModel"/> </summary>
        public TestingLabListModel(IEntityQuery query,
            IEntityBasedModelLoader<TestingLabModel, AbstractLabSchedule> modelLoader,
            ISystemDateService dateService, 
            IGraphLabsPrincipal currentUser) : base(query, modelLoader, dateService, currentUser)
        {
        }

        /// <summary> Режим выполнения </summary>
        protected override LabExecutionMode ExecutionMode => LabExecutionMode.TestMode;

        protected override Expression<Func<AbstractLabSchedule, bool>> GetAdditionalScheduleFilter(IEntityQuery query, Student currentStudent)
        {
            var allSchedules = query.OfEntities<AbstractLabSchedule>();
            var allResults = query.OfEntities<Result>();

            return currentSchedule =>
                 allSchedules
                     .Count(sch => sch.LabWork == currentSchedule.LabWork 
                                && sch.Mode == LabExecutionMode.TestMode
                                && ((sch as IndividualLabSchedule).Student.Id == currentStudent.Id || (sch as GroupLabSchedule).Group.Id == currentStudent.Group.Id))
                     >
                allResults
                     .Count(res => res.LabVariant.LabWork == currentSchedule.LabWork 
                                && res.Student.Id == currentStudent.Id
                                && res.Status != ExecutionStatus.Complete);
        }
    }
}