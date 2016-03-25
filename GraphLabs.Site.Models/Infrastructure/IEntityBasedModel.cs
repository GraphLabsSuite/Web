using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Модель, основанная на сущности </summary>
    public interface IEntityBasedModel<TEntity> where TEntity : AbstractEntity
    {
    }
}