using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.DomainModel.EF.Extensions
{
	public static class UserRoleExtensions
	{
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
