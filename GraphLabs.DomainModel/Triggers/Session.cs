using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using GraphLabs.DomainModel.Extensions;

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
            CreationTime = DateTime.Now;
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            //TODO: добавить проверку для CheckSumSalt
            if (string.IsNullOrWhiteSpace(CheckSum))
                yield return new DbValidationError("CheckSumm", ValidationErrors.Session_OnValidating_Должна_быть_указана_контрольная_сумма_);

            if (LastAction < CreationTime)
                yield return new DbValidationError("LastAction", ValidationErrors.Session_OnValidating_Момент_последнего_действия_не_может_находиться_раньше_момента_создания_сессии_);
            
            if (!IsValidIP(IP))
                yield return new DbValidationError("IP", ValidationErrors.Session_OnValidating_IP_адрес_имеет_неверный_формат_);

            if (entityEntry.State == EntityState.Modified && entityEntry.PropertyChanged("IP"))
                yield return new DbValidationError("IP", ValidationErrors.Session_OnValidating_Нельзя_менять_значение_IP_адреса_);
        }

        private bool IsValidIP(string ip)
        {
            const string PATTERN = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";
            var regex = new Regex(PATTERN);

            return regex.IsMatch(ip);
        }
    }
}
