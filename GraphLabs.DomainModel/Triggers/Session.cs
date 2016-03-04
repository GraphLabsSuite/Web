using System;
using System.Collections.Generic;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Utils;

namespace GraphLabs.DomainModel
{
    /// <summary> Сессия пользователя </summary>
    public partial class Session : AbstractEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public override void OnInsert()
        {
            base.OnInsert();

            Guid = Guid.NewGuid();
        }

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        public override void OnChange(IEntityChange change)
        {
            base.OnChange(change);

            if (change.PropertyChanged("IP"))
                throw new GraphLabsValidationException("IP", ValidationErrors.Session_OnValidating_Нельзя_менять_значение_IP_адреса_);
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (LastAction < CreationTime)
                yield return new EntityValidationError("LastAction", ValidationErrors.Session_OnValidating_Момент_последнего_действия_не_может_находиться_раньше_момента_создания_сессии_);
            
            if (!IpHelper.CheckIsValidIP(IP))
                yield return new EntityValidationError("IP", ValidationErrors.Session_OnValidating_IP_адрес_имеет_неверный_формат_);
        }
    }
}
