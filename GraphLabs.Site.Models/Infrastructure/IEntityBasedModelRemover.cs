using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Сервис удаления моделей из базы </summary>
    /// <typeparam name="TModel">Класс модели</typeparam>
    /// <typeparam name="TEntity">Класс сущности</typeparam>
    public interface IEntityBasedModelRemover<in TModel, out TEntity>
       where TModel : IEntityBasedModel<TEntity>
       where TEntity : AbstractEntity

    {
        /// <summary> Удалить сущность из БД </summary>
        RemovalStatus Remove(long id);
    }


}
