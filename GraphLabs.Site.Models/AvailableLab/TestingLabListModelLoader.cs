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
    public class TestingLabListModelLoader : AvailableLabListModel<TestingLabModel>
    {
        /// <summary> Загрузчик списка <see cref="TestingLabModel"/> </summary>
        public TestingLabListModelLoader(IEntityQuery query,
            IEntityBasedModelLoader<TestingLabModel, AbstractLabSchedule> modelLoader,
            ISystemDateService dateService, 
            IGraphLabsPrincipal currentUser) : base(query, modelLoader, dateService, currentUser)
        {
        }

        /// <summary> Режим выполнения </summary>
        protected override LabExecutionMode ExecutionMode => LabExecutionMode.TestMode;

        protected override Expression<Func<AbstractLabSchedule, bool>> GetAdditionalScheduleFilter(IEntityQuery query, Student currentStudent)
        {
            return currentSchedule =>
                query.OfEntities<AbstractLabSchedule>()
                     .Count(sch => sch.LabWork == currentSchedule.LabWork 
                                && sch.Mode == LabExecutionMode.TestMode
                                && ((sch as IndividualLabSchedule).Student == currentStudent || (sch as GroupLabSchedule).Group == currentStudent.Group))
                     >
                query.OfEntities<Result>()
                     .Count(res => res.LabVariant.LabWork == currentSchedule.LabWork 
                                && res.Student == currentStudent
                                && res.Status != ExecutionStatus.Complete);
        }
    }
}