using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с пользователями </summary>
   // [ContractClass(typeof(UserRepositoryContracts))]
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface IUserRepository : IDisposable
    {
        /// <summary> Получить активного пользователя по email </summary>
        /// <remarks> т.е. подтверждённого, не отчисленного и не удалённого. </remarks>
        [CanBeNull]
        User FindActiveUserByEmail(string email);

        /// <summary> Создать нового студента </summary>
        [NotNull]
        Student CreateNotVerifiedStudent(string email, string name, string fatherName, string surname, string passwordHash, Group group);

		#region Получение массивов пользователей

		/// <summary> Получить массив всех пользователей </summary>
		User[] GetAllUsers();

		/// <summary> Получить массив пользователей-администраторов </summary>
		User[] GetAdministrators();

		/// <summary> Получить массив пользователей-преподавателей </summary>
		User[] GetTeachers();

		/// <summary> Получить массив исключенных студентов </summary>
		Student[] GetDismissedStudents();

		/// <summary> Получить массив подтвержденных студентов </summary>
		Student[] GetVerifiedStudents();

		/// <summary> Получить массив неподтвержденных студентов </summary>
		Student[] GetUnverifiedStudents();

		#endregion

		/// <summary> Получить пользователя по Id </summary>
		User GetUserById(long Id);

		#region Сохранение и редактирование

		/// <summary> Попытка сохранить нового пользователя </summary>
		bool TrySaveUser(User user);

		/// <summary> Попытка изменить пользователя </summary>
		bool TryEditUser(User user);

		/// <summary> Утвердить аккаунт студента </summary>
		void VerifyStudent(long Id);

		/// <summary> Исключить студента </summary>
		void DismissStudent(long Id);

		/// <summary> Восстановить исключенного студента </summary>
		void RestoreStudent(long Id);

		#endregion
	}

	#region Контракты

	///// <summary> Репозиторий с пользователями - контракты </summary>
 //   [ContractClassFor(typeof(IUserRepository))]
 //   internal abstract class UserRepositoryContracts : IUserRepository
 //   {
	//	/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
	//	public void Dispose()
	//	{
	//	}

 //       public User FindActiveUserByEmail(string email)
 //       {
 //           Contract.Requires(!string.IsNullOrWhiteSpace(email));

 //           return default(User);
 //       }

 //       public Student CreateNotVerifiedStudent(string email, string name, string fatherName, string surname, string passwordHash, Group group)
 //       {
 //           Contract.Requires(!string.IsNullOrWhiteSpace(email));
 //           Contract.Requires(!string.IsNullOrWhiteSpace(name));
 //           Contract.Requires(!string.IsNullOrWhiteSpace(surname));
 //           Contract.Requires(!string.IsNullOrWhiteSpace(passwordHash));
 //           Contract.Requires(group != null && group.IsOpen);

 //           Contract.Ensures(Contract.Result<Student>() != null);

 //           return null;
 //       }

	//	#region Получение массивов пользователей

	//	public User[] GetAllUsers()
	//	{
	//		Contract.Ensures(Contract.Result<User[]>() != null);

	//		return new User[0];
	//	}

	//	public User[] GetAdministrators()
	//	{
	//		Contract.Ensures(Contract.Result<User[]>() != null);

	//		return new User[0];
	//	}

	//	public User[] GetTeachers()
	//	{
	//		Contract.Ensures(Contract.Result<User[]>() != null);

	//		return new User[0];
	//	}

	//	public Student[] GetDismissedStudents()
	//	{
	//		Contract.Ensures(Contract.Result<Student[]>() != null);

	//		return new Student[0];
	//	}

	//	public Student[] GetVerifiedStudents()
	//	{
	//		Contract.Ensures(Contract.Result<Student[]>() != null);

	//		return new Student[0];
	//	}

	//	public Student[] GetUnverifiedStudents()
	//	{
	//		Contract.Ensures(Contract.Result<Student[]>() != null);

	//		return new Student[0];
	//	}

	//	#endregion

	//	public User GetUserById(long Id)
	//	{
	//		Contract.Requires(Id > 0);
	//		Contract.Ensures(Contract.Result<User>() != null);

	//		return default(User);
	//	}

	//	#region Сохранение и редактирование

	//	public bool TrySaveUser(User user)
	//	{
	//		Contract.Requires(user != null);

	//		return false;
	//	}

	//	public bool TryEditUser(User user)
	//	{
	//		Contract.Requires(user != null);
	//		return false;
	//	}

	//	public void VerifyStudent(long Id)
	//	{
	//		Contract.Requires(Id > 0);
	//		return;
	//	}

	//	public void DismissStudent(long Id)
	//	{
	//		Contract.Requires(Id > 0);
	//		return;
	//	}

	//	public void RestoreStudent(long Id)
	//	{
	//		Contract.Requires(Id > 0);
	//		return;
	//	}

	//	#endregion
	//}

	#endregion
}
