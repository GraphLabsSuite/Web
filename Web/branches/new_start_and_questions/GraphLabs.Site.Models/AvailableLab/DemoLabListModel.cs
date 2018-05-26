using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> Модель списка демонстрационных лабораторных работ </summary>
    public class DemoLabListModel : AvailableLabListModel<DemoLabModel>
    {
        /// <summary> Модель списка демонстрационных лабораторных работ </summary>
        public DemoLabListModel(IEntityQuery query, 
            IEntityBasedModelLoader<DemoLabModel, AbstractLabSchedule> modelLoader, 
            ISystemDateService dateService,
            IGraphLabsPrincipal currentUser) : base(query, modelLoader, dateService, currentUser)
        {
        }

        /// <summary> Режим выполнения </summary>
        protected override LabExecutionMode ExecutionMode => LabExecutionMode.IntroductoryMode;
    }
}       