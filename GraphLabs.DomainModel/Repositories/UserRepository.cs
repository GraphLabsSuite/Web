using System;
using System.Linq;
using System.Data.Entity;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с пользователями </summary>
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        /// <summary> Репозиторий с пользователями </summary>
        public UserRepository(GraphLabsContext context)
            : base(context)
        {
        }

        /// <summary> Получить активного пользователя по email </summary>
        /// <remarks> т.е. подтверждённого, не отчисленного и не удалённого. </remarks>
        public User FindActiveUserByEmail(string email)
        {
            CheckNotDisposed();

            return Context.Users.SingleOrDefault(u => u.Email == email &&
                                                      (!(u is Student) || ((u as Student).IsVerified && !(u as Student).IsDismissed)));
        }

        /// <summary> Создать нового студента </summary>
        public Student CreateNotVerifiedStudent(string email, string name, string fatherName, string surname, string passwordHash, Group group)
        {
            CheckNotDisposed();

            var student = new Student
            {
                PasswordHash = passwordHash,
                Name = name,
                Surname = surname,
                FatherName = fatherName,
                Email = email,
                IsDismissed = false,
                IsVerified = false,
                Role = UserRole.Student,
                Group = group
            };
            Context.Users.Add(student);

            return student;
		}

		#region Получение массивов пользователей

		/// <summary> Получить массив всех пользователей </summary>
		public User[] GetAllUsers()
		{
			CheckNotDisposed();

			return Context.Users.ToArray();
		}

		/// <summary> Получить массив пользователей-администраторов </summary>
		public User[] GetAdministrators()
		{
			CheckNotDisposed();

			return Context.Users.Where(user => user.Role == UserRole.Administrator).ToArray();
		}

		/// <summary> Получить массив пользователей-преподавателей </summary>
		public User[] GetTeachers()
		{
			CheckNotDisposed();

			return Context.Users.Where(user => user.Role == UserRole.Teacher).ToArray();
		}

		/// <summary> Получить массив исключенных студентов </summary>
		public Student[] GetDismissedStudents()
		{
			CheckNotDisposed();

			return Context.Users
				.Where(user => user.Role == UserRole.Student)
				.ToArray()
				.Select(user => (Student)user)
				.Where(student => student.IsDismissed == true)
				.ToArray();
		}

		/// <summary> Получить массив подтвержденных студентов </summary>
		public Student[] GetVerifiedStudents()
		{
			CheckNotDisposed();

			return Context.Users
				.Where(user => user.Role == UserRole.Student)
				.ToArray()
				.Select(user => (Student)user)
				.Where(student => student.IsDismissed == false)
				.Where(student => student.IsVerified == true)
				.ToArray();
		}

		/// <summary> Получить массив неподтвержденных студентов </summary>
		public Student[] GetUnverifiedStudents()
		{
			CheckNotDisposed();

			return Context.Users
				.Where(user => user.Role == UserRole.Student)
				.ToArray()
				.Select(user => (Student)user)
				.Where(student => student.IsDismissed == false)
				.Where(student => student.IsVerified == false)
				.ToArray();
		}

		#endregion

		/// <summary> Получить пользователя по Id </summary>
		public User GetUserById(long Id)
		{
			return Context.Users.Find(Id);
		}

		#region Сохранение и редактирование

		/// <summary> Попытка сохранить нового пользователя </summary>
		public bool TrySaveUser(User user)
		{
			CheckNotDisposed();

			try
			{
				Context.Users.Add(user);
				Context.SaveChanges();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary> Попытка изменить пользователя </summary>
		public bool TryEditUser(User user)
		{
			CheckNotDisposed();

			try
			{
				Context.Entry(user).State = EntityState.Modified;
				Context.SaveChanges();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
		
		/// <summary> Утвердить аккаунт студента </summary>
		public void VerifyStudent(long Id)
		{
			CheckNotDisposed();

			var user = Context.Users.Find(Id);
			if (user.Role != UserRole.Student)
				throw new InvalidOperationException();

			((Student)user).IsVerified = true;
			Context.Entry(user).State = EntityState.Modified;
			Context.SaveChanges();
		}

		/// <summary> Исключить студента </summary>
		public void DismissStudent(long Id)
		{
			CheckNotDisposed();

			var user = Context.Users.Find(Id);
			if (user.Role != UserRole.Student)
				throw new InvalidOperationException();

			((Student)user).IsDismissed = true;
			Context.Entry(user).State = EntityState.Modified;
			Context.SaveChanges();
		}

		/// <summary> Восстановить исключенного студента </summary>
		public void RestoreStudent(long Id)
		{
			CheckNotDisposed();

			var user = Context.Users.Find(Id);
			if (user.Role != UserRole.Student)
				throw new InvalidOperationException();

			((Student)user).IsDismissed = false;
			Context.Entry(user).State = EntityState.Modified;
			Context.SaveChanges();
		}

		#endregion
	}
}