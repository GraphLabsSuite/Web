using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Controllers.LabWorks
{
    public class ResponseConstants
	{
		#region Константы ответов создания и редактирования лабораторных работ

		/// <summary> Лабораторная работа успешно создана </summary>
		public const int LabWorkSuccessCreateSystemName = 0;

		/// <summary> Лаборатораня работа успешно обновлена </summary>
		public const int LabWorkSuccessEditSystemName = 1;

		/// <summary> Лабораторная работа с таким именем уже существует </summary>
		public const int LabWorkExistErrorSystemName = 2;		

		#endregion

		#region Константы ответов создания и редактирования вариантов лабораторной работы

		/// <summary> Вариант л.р. успешно сохранен </summary>
		public const int LabVariantSaveSuccessSystemName = 0;

		/// <summary> Вариант л.р. успешно обновлен </summary>
		public const int LabVariantModifySuccessSystemName = 1;

		/// <summary> Вариант л.р. с таким именем уже существует </summary>
		public const int LabVariantNameCollisionSystemName = 2;

		/// <summary> Ошибка сохранения в БД </summary>
		public const int LabVariantSaveErrorSystemName = 3;

		/// <summary> Ошибка изменения в БД </summary>
		public const int LabVariantModifyErrorSystemName = 4;

		#endregion
	}
}