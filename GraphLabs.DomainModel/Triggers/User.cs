using System.Collections.Generic;
using System.Text.RegularExpressions;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Пользователь </summary>
    public partial class User : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (!IsValidEmail(Email))
                yield return new EntityValidationError("Email", ValidationErrors.User_OnValidating_Указан_неверный_Email_адрес_);

            if (string.IsNullOrWhiteSpace(Name))
                yield return new EntityValidationError("Name", ValidationErrors.User_OnValidating_Необходимо_указать_имя_пользователя_);

            if (string.IsNullOrWhiteSpace(PasswordHash))
                yield return new EntityValidationError("PasswordHash", ValidationErrors.User_OnValidating_Должно_быть_указано_значение_PasswordHash_);

            if (string.IsNullOrWhiteSpace(Surname))
                yield return new EntityValidationError("Surname", ValidationErrors.User_OnValidating_Необходимо_указать_фамилию_пользователя_);
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
