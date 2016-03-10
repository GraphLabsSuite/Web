namespace GraphLabs.DomainModel.Extensions
{
    /// <summary> Расширения для пользователей </summary>
    public static class UserExtensions
    {
        /// <summary> Возращает имя пользователя в формате Фамилия И.О. </summary>
        public static string GetShortName(this User user)
        {
            return user.FatherName != null 
                ? string.Format("{0} {1}.{2}.", user.Surname, user.Name[0], user.FatherName[0])
                : string.Format("{0} {1}.", user.Surname, user.Name[0]);
        }

    }
}
