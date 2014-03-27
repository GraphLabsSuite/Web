using System;
using System.Linq;

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
    }
}