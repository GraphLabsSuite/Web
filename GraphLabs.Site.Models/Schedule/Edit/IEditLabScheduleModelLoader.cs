using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Startpage.Edit
{
    public interface IEditLabScheduleModelLoader : IEntityBasedModelLoader<EditLabScheduleModelBase, AbstractLabSchedule>
    {
        EditLabScheduleModelBase CreateEmptyModel(EditLabScheduleModelBase.Kind kind);
    }
}