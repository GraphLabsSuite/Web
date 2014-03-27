namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Пользовательские роли </summary>
    public static class UserRoles
    {
        /// <summary> Студент </summary>
        public static readonly string Student = DomainModel.UserRole.Student.ToString();
        
        /// <summary> Преподаватель </summary>
        public static readonly string Teacher = DomainModel.UserRole.Teacher.ToString();

        /// <summary> Администратор </summary>
        public static readonly string Administrator = DomainModel.UserRole.Administrator.ToString();
    }
}