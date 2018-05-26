using GraphLabs.DomainModel;

namespace GraphLabs.Dal.Ef.Extensions
{
    /// <summary> Расширения для пользовательских ролей </summary>
	public static class UserRoleExtensions
	{
        /// <summary> Значение -> строка </summary>
		public static string ValueToString(this UserRole role)
		{
			switch (role)
			{
				case UserRole.Administrator:
					return "4";
				case UserRole.Teacher:
					return "2";
				case UserRole.Student:
					return "1";
				default:
					return "0";
			}
		}
	}
}
