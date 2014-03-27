using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;

namespace GraphLabs.DomainModel
{
    /// <summary> Пользователь </summary>
    public partial class User : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (!IsValidEmail(Email))
                yield return new DbValidationError("Email", ValidationErrors.User_OnValidating_Указан_неверный_Email_адрес_);

            if (string.IsNullOrWhiteSpace(Name))
                yield return new DbValidationError("Name", ValidationErrors.User_OnValidating_Необходимо_указать_имя_пользователя_);

            if (string.IsNullOrWhiteSpace(PasswordHash))
                yield return new DbValidationError("PasswordHash", ValidationErrors.User_OnValidating_Должно_быть_указано_значение_PasswordHash_);

            if (string.IsNullOrWhiteSpace(Surname))
                yield return new DbValidationError("Surname", ValidationErrors.User_OnValidating_Необходимо_указать_фамилию_пользователя_);
        }

        private bool IsValidEmail(string email)
        {
            const string PATTERN = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                   + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                   + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(PATTERN, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }
    }
}
